using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using ControlzEx.Standard;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Дипломная_работа___Гимаев_Амир.Windows;
using Image = System.Drawing.Image;
using Point = System.Windows.Point;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class AForgeDocumentDisplay
    {

        private static MainWindow _mainWindow;



        private static VideoCaptureDevice _videoFrame; // Получает изображение с устройства.

        private static FilterInfoCollection _devices; // Хранит список доступных устройств.

        public static string CurrentDevice; // Хранит "прозвище" текущего устройства вывода изображения.

        public static System.Windows.Media.PixelFormat CurrentPixelFormat; // Цветность отображаемого изображения.

        public static ImageFormat ScanFormat; // Расширение для снимков.


        /// <summary>
        /// Инициализация всех необходимых экземпляров.
        /// </summary>
        /// <param name="_mw">Экземпляр окна MainWindow</param>
        public static void Initialize(in MainWindow _mw)
        {
            _mainWindow = _mw;

                _devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (_devices.Count == 0) return ; // Если ПО не нашел не одного устройства, то он метод прекращает выполнение.

                foreach (FilterInfo _device in _devices) _mainWindow.SelectDevice.Items.Add(_device.Name); // добавляет имя камеры в элемент ComboBox "SelectDevice"

                List<FilterInfo> _listfilterInfos = new List<FilterInfo>(); // Создается List для дальнейщего переобразования в массив. 

                foreach (FilterInfo fi in _devices) _listfilterInfos.Add(fi);

                FilterInfo[] _filterInfos = _listfilterInfos.ToArray(); // Этот массив создается для поиска нужной камеры по имени

                if (CurrentDevice != string.Empty)
                {
                    _videoFrame = new VideoCaptureDevice
                        (Array.Find(_filterInfos, d => d.Name == CurrentDevice).MonikerString);

                    _mainWindow.SelectDevice.SelectedValue = CurrentDevice;
                }

                else
                {
                    _videoFrame = new VideoCaptureDevice(_devices[0].MonikerString);

                    _mainWindow.SelectDevice.SelectedIndex = 0;
                }

                _videoFrame.NewFrame += new NewFrameEventHandler(NewFrameWithEffect);

                _videoFrame.Start();

        }


        /// <summary>
        /// Проверка на наличие хотя бы одной камеры.
        /// </summary>
        /// <returns>Если найдена камера, то возвращает true, в противном случае - false</returns>
        public static bool CheckVideoInputDevice()
        {
            _devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (_devices.Count == 0) return false; else return true;
        }


        public static class SearchDevices
        {
            static private LoadingScreen _loadingScreen;

            static private HwndSource _hwndSource;

            /// <summary>
            /// Поиск устройтсва для вывода видеопотока.
            /// </summary>
            /// <param name="_window">Экземпляр окна LoadingScreen</param>
            public static void SearchDevice(LoadingScreen _window)
            {
                _loadingScreen = _window;

                _loadingScreen.LoadRing.IsActive = false;

                _loadingScreen.Progress.Text = "Камера не найдена. Убедитесь что устройство подключено и исправно.";

                _hwndSource = HwndSource.FromVisual(_window) as HwndSource;

                if (_hwndSource != null)
                {
                    IntPtr windowHandle = _hwndSource.Handle;
                    _hwndSource.AddHook(UsbNotificationHandler); 
                }
            }

            static private IntPtr UsbNotificationHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                if (msg == 0x0219 & (int)wParam == 7) // Если будет подключено или отключено (!любое!) устройство.
                {
                    _devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                    if (_devices.Count > 0)
                    {
                        _hwndSource.RemoveHook(UsbNotificationHandler);
                        _hwndSource.Dispose();
                        _loadingScreen.LoadRing.IsActive = true;
                        _loadingScreen.Progress.Text = "Загрузка...";
                        _loadingScreen.MainWindowInitializetion();
                    }
                }
                return (IntPtr)0;
            }
        }



        // Вывод видеопотока с камеры на экран.
        private static void NewFrameWithEffect(object sender, NewFrameEventArgs e)
        {
            BitmapImage _bi; 

            FormatConvertedBitmap _biGray;

            using (var bitmap = e.Frame.Clone() as Bitmap)
            {
                BrightnessCorrection _brightness = new BrightnessCorrection((int)ProgrammSetting.PhotoSetting.Brightness);
                _brightness.ApplyInPlace(bitmap);

                ContrastCorrection _contrast = new ContrastCorrection((int)ProgrammSetting.PhotoSetting.Contrast);
                _contrast.ApplyInPlace(bitmap);

                _bi = new BitmapImage();
                _bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                _bi.StreamSource = ms;
                _bi.CacheOption = BitmapCacheOption.OnLoad;
                _bi.EndInit();

                _biGray = new FormatConvertedBitmap();
                _biGray.BeginInit();
                _biGray.Source = _bi;
                _biGray.DestinationFormat = CurrentPixelFormat;
                _biGray.EndInit();
                
            }
            _biGray.Freeze();
            Application.Current.Dispatcher.Invoke(new ThreadStart(delegate { _mainWindow.FrameArea.Source = _biGray; }));
        }



        /// <summary>
        /// Выбор устройства для вывода видеопотока на экран.
        /// </summary>
        /// <param name="index">Индекс выбранного устройства</param>
        public static void SelectDevice(in int index)
        {
            _videoFrame.Stop();
            _videoFrame = new VideoCaptureDevice(_devices[index].MonikerString); 
            _videoFrame.Start();
        }



        /// <summary>
        /// Останавливает работу AForge во время закрытия программы.
        /// </summary>        
        public static void StopFrame() 
        {
            if (_videoFrame != null & _videoFrame.IsRunning)
            {
                _videoFrame.SignalToStop();
                _videoFrame.NewFrame -= NewFrameWithEffect;
                _videoFrame.WaitForStop();
                _videoFrame = null;
            }    
        }




        private static uint _photoNumber = 0; // порядковый номер снимка

        /// <summary>
        /// Делает снимок документа.
        /// </summary>
        public static void TakeASnapshotOfTheDocument()
        {
            Vector _difference1 = _mainWindow.ScanArea.PointToScreen(new Point(0, 0)) - _mainWindow.MovingSpaceCanvas.PointToScreen(new Point(0, 0));

            // Подготовка экземляра класса Crop для обрезки снимка
            Crop _crop = new Crop(new Rectangle
                    (Convert.ToInt32(_difference1.X) + 5, Convert.ToInt32(_difference1.Y + 5),
            Convert.ToInt32(_mainWindow.ScanArea.Width - 5), Convert.ToInt32(_mainWindow.ScanArea.Height - 5)));
            RenderTargetBitmap _rtb = new RenderTargetBitmap
                ((int)_mainWindow.FrameArea.ActualWidth, (int)_mainWindow.FrameArea.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            _rtb.Render(_mainWindow.FrameArea);

            // Сохранение снимка
            PngBitmapEncoder _png = new PngBitmapEncoder();
            _png.Frames.Add(BitmapFrame.Create(_rtb));
            MemoryStream _stream = new MemoryStream();
            _png.Save(_stream);
            Image _image = Image.FromStream(_stream);

            // Обрезка снимка
            Bitmap _newImage = _crop.Apply((Bitmap)_image);

            string _currentPathFile = $@"{_mainWindow.ScanPath.Text}\{_mainWindow.ScanName.Text}_{_photoNumber}.{ScanFormat}";

            if (!SavePhoto(_newImage)) return;

            AddPhotoToListOfPhotos(new string[] { _currentPathFile });
        }

        
        private static bool SavePhoto(Bitmap _image)
        {
            try
            {
                // Сохранение снимка в заданном папке

                if (_mainWindow.ScanPath.Text != string.Empty)
                    if (_mainWindow.ScanName.Text != string.Empty)
                    {
                        _image.Save($@"{_mainWindow.ScanPath.Text}\{_mainWindow.ScanName.Text}_{_photoNumber}.{ScanFormat}", ScanFormat); 
                        _photoNumber++;
                        return true;
                    }
                    else { _mainWindow.ShowMessageAsync("Оцифровка документов", "Задайте постоянную часть имени файла снимка"); return false; }
                else { _mainWindow.ShowMessageAsync("Оцифровка документов", "Задайте путь к папке для сохранения снимков"); return false; }
            }
            catch (Exception)
            {
                _photoNumber++;
                SavePhoto(_image);

                return false; // ???...
            }
        }

        // Добавляет снимки в ListOfPhotos
        private static void AddPhotoToListOfPhotos(string[] _fullPathPhotos)
        {
            foreach (string _fullPathPhoto in _fullPathPhotos)
            {
                // Если файл не является изображением, то произойдет выход из метода
                if (!_fullPathPhoto.EndsWith(".Png") & !_fullPathPhoto.EndsWith(".Jpeg") 
                    & !_fullPathPhoto.EndsWith(".Tiff") & !_fullPathPhoto.EndsWith(".Bmp")) return; 

                // В переменную _bi добавим ранне созданый снимок (для того, чтобы добавить этот снимок в ListOfPhotos)
                var _bi = new BitmapImage();
                _bi.BeginInit();
                _bi.UriSource = new Uri(_fullPathPhoto);
                _bi.EndInit();

                // В _sp будет храниться элемент Image (со снимком) и TextBlok (с кратким именем снимка)
                StackPanel _sp = new StackPanel() { Margin = new Thickness(5)};
                _sp.MaxHeight = 120;
                _sp.Children.Add(new System.Windows.Controls.Image() { Stretch = Stretch.Uniform, MaxHeight = 100, MaxWidth = 100 });
                _sp.Children.Add(new TextBlock());

                (_sp.Children[0] as System.Windows.Controls.Image).Source = _bi;
                (_sp.Children[1] as TextBlock).HorizontalAlignment = HorizontalAlignment.Center;
                (_sp.Children[1] as TextBlock).Text = _fullPathPhoto.Remove(0,_fullPathPhoto.LastIndexOf('\\') + 1);

                ListBoxItem lbi = new ListBoxItem() { Content = _sp };

                _mainWindow.ListOfPhotos.Items.Add(lbi);
            }
        }


        /// <summary>
        /// Добавляет снимки, перетаскиваемые из рабочего стола в ListOfPhotos.
        /// </summary>
        /// <param name="e">Данный параметр передает данные об переметаскиваемых файлах.</param>
        public static void DropPhotosToListOfPhotos(DragEventArgs e)
        {
            string[] _files = e.Data.GetData(DataFormats.FileDrop, true) as string[];

            AddPhotoToListOfPhotos(_files);
        }

        /// <summary>
        /// Выбор цветности снимка.
        /// </summary>
        /// <param name="e">Данный параметр передает данные об выбранном цветности</param>
        public static void SelectChroma(SelectionChangedEventArgs e)
        {
            switch((e.AddedItems[0] as ComboBoxItem).Content as string)
            {
                case "Цветной":
                    CurrentPixelFormat = PixelFormats.Default;
                    break;

                case "Оттенки серого":
                    CurrentPixelFormat = PixelFormats.Gray32Float;
                    break;

                case "Черно-белый":
                    CurrentPixelFormat = PixelFormats.BlackWhite;
                    break;
            }
        }

        /// <summary>
        /// Выбор формата снимка.
        /// </summary>
        /// <param name="e">Данный параметр передает данные об выбранном формате изображений</param>
        public static void SelectFormat(SelectionChangedEventArgs e)
        {
            switch ((e.AddedItems[0] as ComboBoxItem).Content as string) 
            {
                case "JPG":
                    ScanFormat = ImageFormat.Jpeg;
                    break;
                case "PNG":
                    ScanFormat = ImageFormat.Png;
                    break;
                case "BMP":
                    ScanFormat = ImageFormat.Bmp;
                    break;
                case "TIF":
                    ScanFormat = ImageFormat.Tiff;
                    break;
            }
        }
    }
}
