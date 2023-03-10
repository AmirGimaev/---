using MahApps.Metro.Controls;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Дипломная_работа___Гимаев_Амир.Classes;

namespace Дипломная_работа___Гимаев_Амир
{
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
            ScanAreaClass.ScanAreaCenterLBM(sender, e);
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

        private void OpenSettingPage(object sender, RoutedEventArgs e)
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
            PDFClass.SelectFolderPath(ScanPath);
        }

        private void TakeASnapshot(object sender, RoutedEventArgs e)
        {
            AForgeDocumentDisplay.TakeASnapshotOfTheDocument();
        }


        private void AddPicturesToListOfPhotos(object sender, DragEventArgs e)
        {
            ListOfPhotosClass.DropPhotosToListOfPhotos(ref ListOfPhotos, e);
        }

        private void ChangeImageFormat(object sender, SelectionChangedEventArgs e)
        {
            AForgeDocumentDisplay.SelectFormat(e);
        }

        private void RemoveSelectionsInListOfPhotos(object sender, MouseButtonEventArgs e)
        {
            ListOfPhotosClass.RemoveSelected();
        }

        private void SaveAsPDFfile(object sender, RoutedEventArgs e)
        {
            PDFClass.OpenPDFFormatParametrsWindow(ListOfPhotos, ScanPath.Text + '\\', this);
        }

        private void OpenPDFEditor(object sender, RoutedEventArgs e)
        {
            PDFClass.OpenPDFEditor();
        }


        private void RemoveSelectedItems(object sender, RoutedEventArgs e)
        {
            ListOfPhotosClass.DeleteSelectedImages(ListOfPhotos);
        }

        private void TakeSnapshotWithTimer(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(Convert.ToInt32(ShotTimerTextBox.Value + "000"));
            AForgeDocumentDisplay.TakeASnapshotOfTheDocument();
        }

        private void OpenImagesFromCatalog(object sender, RoutedEventArgs e)
        {
            ListOfPhotosClass.OpenImagesFromCatalog(ref ListOfPhotos, ref ScanPath);
        }
    }
}
