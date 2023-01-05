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
            ProgrammSetting.mainWindow = this;
            ProgrammSetting.ReadSettingFileAndSettingProgramm();

            InitializeComponent();
            
            CropRectangleClass.mainWindow = this;
            CropRectangleClass.InitializationCropRectangleClass();

            // Добавление обработчика события, который выделяет весь список при нажатии клавишь Ctrl + A
            ListOfPhotos.InputBindings.Add(new KeyBinding(ApplicationCommands.SelectAll,
                          new KeyGesture(Key.A, ModifierKeys.Control)));
            ListOfPhotos.CommandBindings.Add(new CommandBinding(ApplicationCommands.SelectAll, (_sender, _e) =>
            {
                ListOfPhotos.SelectAll();
            }));

            DataContext = ProgrammSetting.MainWindowsStyleSetting.Font;
        }


        private void MovingSpace_Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CropRectangleClass.CropRectangleAdaptSize(sender, e);
        }

        private void CropRectangle_left_top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleLeftTopLBM(sender, e);
        }

        private void CropRectangle_right_top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleRightTopLBM(sender, e);
        }

        private void CropRectangle_botton_left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleLeftBottonLBM(sender, e);
        }

        private void CropRectangle_botton_right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleRightBottonLBM(sender, e);
        }









     

        private void CropRectangle_top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleTopLBM(sender, e);
        }

        private void CropRectangle_left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleLeftLBM(sender, e);
        }

        private void CropRectangle_right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleRightLBM(sender, e);
        }

        private void CropRectangle_botton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleBottonLBM(sender, e);
        }






















        private void CropRectangle_center_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.CropRectangleCenterLBM(sender,e);
        }







        private void StopChangeSizeAndDragCropRectangle(object sender, MouseButtonEventArgs e)
        {
            CropRectangleClass.RemoveAllCanvasMouseEventArgs();
        }
        private void StopChangeSizeAndDragCropRectangle(object sender, MouseEventArgs e)
        {
             CropRectangleClass.RemoveAllCanvasMouseEventArgs();
        }

        private void SettingButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e) 
            => SettingPage.IsOpen = true;
    }
}
