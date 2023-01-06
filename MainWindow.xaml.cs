using ControlzEx.Standard;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Дипломная_работа___Гимаев_Амир.Classes;

namespace Дипломная_работа___Гимаев_Амир
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MovingSpace_Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScanAreaClass.ScanAreaAdaptSize(sender, e);
        }

        private void ScanArea_left_top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaLeftTopLBM(sender, e);
        }

        private void ScanArea_right_top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaRightTopLBM(sender, e);
        }

        private void ScanArea_botton_left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaLeftBottonLBM(sender, e);
        }

        private void ScanArea_botton_right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaRightBottonLBM(sender, e);
        }









     

        private void ScanArea_top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaTopLBM(sender, e);
        }

        private void ScanArea_left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaLeftLBM(sender, e);
        }

        private void ScanArea_right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaRightLBM(sender, e);
        }

        private void ScanArea_botton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaBottonLBM(sender, e);
        }






















        private void ScanArea_center_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.ScanAreaCenterLBM(sender,e);
        }







        private void StopChangeSizeAndDragScanArea(object sender, MouseButtonEventArgs e)
        {
            ScanAreaClass.RemoveAllCanvasMouseEventArgs();
        }
        private void StopChangeSizeAndDragScanArea(object sender, MouseEventArgs e)
        {
             ScanAreaClass.RemoveAllCanvasMouseEventArgs();
        }

        

        private void SelectTheme(object sender, SelectionChangedEventArgs e)
        {
            ProgrammSetting.MainWindowsStyleSetting.ChangeTheme();
        }

        private void SettingButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e) 
            => SettingPage.IsOpen = true;

        private void MetroWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProgrammSetting.WriteSettingFileNewSettings();

            AForgeDocumentDisplay.StopFrame();
        }

        private void MetroWindowInitialized(object sender, EventArgs e)
        {
            ProgrammSetting.MainWindowInitialization(sender as MainWindow);
        }

        private void SelectChroma(object sender, SelectionChangedEventArgs e)
        {
            AForgeDocumentDisplay.SelectChroma(e);
        }

        private void ContrastValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgrammSetting.PhotoSetting.Contrast = Convert.ToInt32(e.NewValue);
        }

        private void BrightnessValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgrammSetting.PhotoSetting.Brightness = Convert.ToInt32(e.NewValue);
        }


        private void SelectPathForPhoto(object sender, RoutedEventArgs e)
        {
            PDF.SelectFolderPath(ScanPath);
        }
    }
}
