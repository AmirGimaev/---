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



        private static VideoCaptureDevice VideoFrame; // Получает изображение с устройства

        private static FilterInfoCollection Devices; // Хранит список доступных устройств

        public static string CurrentDevice; // Хранит "прозвище" текущего устройства вывода изображения

        public static System.Windows.Media.PixelFormat CurrentPixelFormat;

        // Инициализация всех необходимых экземпляров
        public static void Initialize(in MainWindow _mw)
        {
            _mainWindow = _mw;

                Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (Devices.Count == 0) return ;

                foreach (FilterInfo _device in Devices) _mainWindow.SelectDevice.Items.Add(_device.Name);

                List<FilterInfo> _listfilterInfos = new List<FilterInfo>();
                foreach (FilterInfo fi in Devices) _listfilterInfos.Add(fi);
                FilterInfo[] _filterInfos = _listfilterInfos.ToArray();

                if (CurrentDevice != string.Empty)
                {
                    VideoFrame = new VideoCaptureDevice
                        (Array.Find(_filterInfos, d => d.Name == CurrentDevice).MonikerString);

                    _mainWindow.SelectDevice.SelectedValue = CurrentDevice;
                }

                else
                {
                    VideoFrame = new VideoCaptureDevice(Devices[0].MonikerString);

                    _mainWindow.SelectDevice.SelectedIndex = 0;
                }

                VideoFrame.NewFrame += new NewFrameEventHandler(NewFrameWithEffect);

                VideoFrame.Start();

        }

        // Проверка на наличие хотя бы одной камеры
        public static bool CheckVideoInputDevice()
        {
            Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (Devices.Count == 0) return false; else return true;
        }


        public static class SearchDevices
        {
            static private LoadingScreen _loadingScreen;

            static private HwndSource hwndSource;

            public static void SearchDevice(LoadingScreen _window)
            {
                _loadingScreen = _window;

                _loadingScreen.LoadRing.IsActive = false;

                _loadingScreen.Progress.Text = "Камера не найдена. Убедитесь что устройство подключено и исправно.";

                hwndSource = HwndSource.FromVisual(_window) as HwndSource;

                if (hwndSource != null)
                {
                    IntPtr windowHandle = hwndSource.Handle;
                    hwndSource.AddHook(UsbNotificationHandler); 
                }
            }

            static private IntPtr UsbNotificationHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                if (msg == 0x0219 & (int)wParam == 7) // Если будет подключено или отключено (!любое!) устройство.
                {
                    Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    if (Devices.Count > 0)
                    {
                        hwndSource.RemoveHook(UsbNotificationHandler);
                        hwndSource.Dispose();
                        _loadingScreen.Progress.Text = "Загрузка...";
                        _loadingScreen.MainWindowInitializetion();
                    }
                }
                return (IntPtr)0;
            }
        }




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
                bitmap.Save(ms, ImageFormat.Bmp);
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


        public static void SelectDevice(in int index)
        {
            VideoFrame.Stop();
            VideoFrame = new VideoCaptureDevice(Devices[index].MonikerString); 
            VideoFrame.Start();
        }

        public static void StopFrame() 
        {
            Application.Current.Dispatcher.Invoke(new ThreadStart(delegate
            {
                if (!(VideoFrame == null))
                    if (VideoFrame.IsRunning)
                    {
                        VideoFrame.SignalToStop();
                        VideoFrame = null;
                    }
            }));
        }
        
        public static void TakeASnapshotOfTheDocument()
        {
            Vector difference1 = _mainWindow.ScanArea.PointToScreen(new Point(0, 0)) - _mainWindow.MovingSpaceCanvas.PointToScreen(new Point(0, 0));

            // Подготовка экземляра класса Crop для обрезки снимка
            Crop crop = new Crop(new Rectangle
                    (Convert.ToInt32(difference1.X) + 5, Convert.ToInt32(difference1.Y + 5),
            Convert.ToInt32(_mainWindow.ScanArea.Width - 5), Convert.ToInt32(_mainWindow.ScanArea.Height - 5)));
            RenderTargetBitmap rtb = new RenderTargetBitmap
                ((int)_mainWindow.FrameArea.ActualWidth, (int)_mainWindow.FrameArea.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(_mainWindow.FrameArea);

            // Сохранение снимка
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            MemoryStream stream = new MemoryStream();
            png.Save(stream);
            Image image = Image.FromStream(stream);

            // Обрезка снимка
            Bitmap newImage = crop.Apply((Bitmap)image);

            // Сохранение снимка в заданном папке
            newImage.Save($@"C:\Users\Amir\Desktop\{_mainWindow.ScanName.Text}.png");

            // В переменную _bi добавим ранне созданый снимок (для того чтобы добавить этот снимок в ListOfPhotos)
            var _bi = new BitmapImage();
            _bi.BeginInit();
            _bi.UriSource = new Uri($@"C:\Users\Amir\Desktop\{_mainWindow.ScanName.Text}.png");
            _bi.EndInit();

            // В _sp будет храниться элемент Image (со снимком) и TextBlok (с кратким именем снимка)
            StackPanel _sp = new StackPanel();
            _sp.MaxHeight = 120;
            _sp.Children.Add(new System.Windows.Controls.Image() { Stretch = Stretch.Uniform, MaxHeight = 100, MaxWidth = 100 });
            _sp.Children.Add(new TextBlock());

            (_sp.Children[0] as System.Windows.Controls.Image).Source = _bi;
            (_sp.Children[1] as TextBlock).Text = _mainWindow.ScanName.Text;

            ListBoxItem lbi = new ListBoxItem() { Content = _sp };

            _mainWindow.ListOfPhotos.Items.Add(lbi);
        }

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
    }
}
