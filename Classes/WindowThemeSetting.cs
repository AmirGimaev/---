using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    internal class WindowThemeSetting
    {
        private static string[,] Settings = new string[10,2];
        public static Brush MainWindowBackgroundColor, ToolsBackgroundColor; // Цвет заднего фона главного окна и окон инстументов
        public static string StyleTitle; // Стиль окна
        public static int Height, Widht; // Размер окна
        public static string LastPathForPhotos, LastNameForPhoto, LastNameForPDF; // Последнее использованные имена
        public static int CropHeight, CropWight;
        public static Orientation OrientationFrames; // Оринтация кадров
        public static TimeSpan SnapshotTime;

        public static void SetTheme()
        {
            MainWindow MainWin = new MainWindow() 
            {
                Height = Height,
                Width = Widht,
                Background = MainWindowBackgroundColor
            };

            ThemeManager.Current.ChangeTheme(MainWin, StyleTitle);




        }
    }
}
