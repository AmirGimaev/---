using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.SessionState;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
            try
            {
                Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                foreach(FilterInfo _device in Devices) _mainWindow.SelectDevice.Items.Add(_device.Name);

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
            catch (ArgumentOutOfRangeException) 
            {
                MessageBox.Show("Веб камера не найдена. \n\n Убедитесь, что устройство исправна и подключена к системе.",
                "Оцифровка докуметов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void NewFrameWithEffect(object sender, NewFrameEventArgs e)
        {
            BitmapImage _bi; FormatConvertedBitmap _biGray;
            using (var bitmap = e.Frame.Clone() as Bitmap)
            {
                BrightnessCorrection _brightness = new BrightnessCorrection(ProgrammSetting.PhotoSetting.Brightness);
                _brightness.ApplyInPlace(bitmap);

                ContrastCorrection _contrast = new ContrastCorrection(ProgrammSetting.PhotoSetting.Contrast);
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
