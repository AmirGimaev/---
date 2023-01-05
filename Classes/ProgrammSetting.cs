using ControlzEx.Theming;
using System;
using System.IO;
using System.Windows.Media;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    internal class ProgrammSetting
    {

        public static MainWindow mainWindow; // Здесь храниться экземпляр главного окна




        static public class MainWindowsStyleSetting 
        {
            public static Color MainWindowBackgroundColor, ToolsBackgroundColor; // Цвет заднего фона главного окна и окон инстументов

            public static string StyleTitleShade, StyleTitleColor; // Стиль окна

            public static int Height, Widht; // Размер окна

            public static string Font; // Шрифт


            public static void ChangeTheme()
            {
                mainWindow.Height = Height;
                mainWindow.Width = Widht;
                mainWindow.Background = new SolidColorBrush(MainWindowBackgroundColor);

                mainWindow.DataContext = Font;

                ThemeManager.Current.ChangeTheme(mainWindow, StyleTitleShade + "." + StyleTitleColor);
            }
        }




        static public class CropRectangleSetting 
        {
            public static int CropHeight, CropWight;

            public static int RotateFrames; // Оринтация кадров

        }




        static public class PhotoSetting 
        {
            public static string LastPathForPhotos, LastNameForPhoto, LastNameForPDF; // Последнее использованные имена

            public static TimeSpan SnapshotTime; // Время для таймера

        }


        public static void ReadSettingFileAndSettingProgramm() // Метод для чтения всех настроек с файла Setting.txt
        {
            string currentDirectory = Environment.CurrentDirectory; // Текущая директория

            FileInfo settingFile = new FileInfo(currentDirectory + "\\Setting.txt");

            if (settingFile.Exists)
            {
                // Считывает все значения параметров с файла
                string[] settingFromSettingFile = File.ReadAllLines(settingFile.FullName);


                // Получение последних сохраненных значений настройки
                string[] formatedSetting = new string[settingFromSettingFile.Length];
                for (byte i = 0; i < settingFromSettingFile.Length; i++)
                {
                    formatedSetting[i] = settingFromSettingFile[i].
                        Remove(0, settingFromSettingFile[i].IndexOf('=') + 1);
                }



                // Сохраненный цвет заднего фона главного окна
                string[] mainWindowBC = formatedSetting[0].Split(',');
                byte[] mainWindowBackgroundColor = { Convert.ToByte(mainWindowBC[0]), Convert.ToByte(mainWindowBC[1]),
                    Convert.ToByte(mainWindowBC[2]), Convert.ToByte(mainWindowBC[3]) };

                MainWindowsStyleSetting.MainWindowBackgroundColor = Color.FromArgb(mainWindowBackgroundColor[0],
                    mainWindowBackgroundColor[1], mainWindowBackgroundColor[2], mainWindowBackgroundColor[3]);





                // Сохраненный цвет заднего фона окон инструментов
                string[] toolsBC = formatedSetting[1].Split(',');
                byte[] toolsBackgroundColor = { Convert.ToByte(toolsBC[0]), Convert.ToByte(toolsBC[1]),
                    Convert.ToByte(toolsBC[2]), Convert.ToByte(toolsBC[3]) };

                MainWindowsStyleSetting.ToolsBackgroundColor = Color.FromArgb(toolsBackgroundColor[0],
                    toolsBackgroundColor[1], toolsBackgroundColor[2], toolsBackgroundColor[3]);





                // Сохраненный стиль окна
                MainWindowsStyleSetting.StyleTitleShade = formatedSetting[2];

                MainWindowsStyleSetting.StyleTitleColor = formatedSetting[3];



                // 
                MainWindowsStyleSetting.Height = Convert.ToInt32(formatedSetting[4]);

                MainWindowsStyleSetting.Widht = Convert.ToInt32(formatedSetting[5]);





                PhotoSetting.LastPathForPhotos = formatedSetting[6];

                PhotoSetting.LastNameForPhoto = formatedSetting[7];

                PhotoSetting.LastNameForPDF = formatedSetting[8];



                CropRectangleSetting.CropHeight = Convert.ToInt32(formatedSetting[9]);

                CropRectangleSetting.CropWight = Convert.ToInt32(formatedSetting[10]);



                CropRectangleSetting.RotateFrames = Convert.ToInt32(formatedSetting[11]);



                PhotoSetting.SnapshotTime = new TimeSpan(0, 0, Convert.ToInt32(formatedSetting[12]));

                MainWindowsStyleSetting.Font = formatedSetting[13];


            }
            else
            {
                settingFile.Create();
            }

        }


        public static void WriteSettingFileNewSettings() // Метод для записи в файл всех настроек
        {
            string currentDirectory = Environment.CurrentDirectory; // Текущая директория

            FileInfo settingFile = new FileInfo(currentDirectory + "\\Setting.txt");

            if (!settingFile.Exists) settingFile.Create();

            string setting = 
                $"MetroWindow.BackgroundColor={MainWindowsStyleSetting.MainWindowBackgroundColor.A},{MainWindowsStyleSetting.MainWindowBackgroundColor.R}," +
                    $"{MainWindowsStyleSetting.MainWindowBackgroundColor.G},{MainWindowsStyleSetting.MainWindowBackgroundColor.B}\r\n" +
                $"Tools.BackgroundColor={MainWindowsStyleSetting.ToolsBackgroundColor.A},{MainWindowsStyleSetting.ToolsBackgroundColor.R}," +
                    $"{MainWindowsStyleSetting.ToolsBackgroundColor.G},{MainWindowsStyleSetting.ToolsBackgroundColor.B}\r\n" +
                $"StyleTheme={MainWindowsStyleSetting.StyleTitleShade}\r\n" +
                $"TitleColor={MainWindowsStyleSetting.StyleTitleColor}\r\n" +
                $"MetroWindowHeight={mainWindow.ActualHeight}\r\n" +
                $"MetroWindowWidht={mainWindow.ActualWidth}\r\n" +
                $"LastPathForPhotos={PhotoSetting.LastPathForPhotos}\r\n" +
                $"LastNameForPhoto={PhotoSetting.LastNameForPhoto}\r\n" +
                $"LastNameForPDF={PhotoSetting.LastNameForPDF}\r\n" +
                $"CropHeight={CropRectangleSetting.CropHeight}\r\n" +
                $"CropWidth={CropRectangleSetting.CropWight}\r\n" +
                $"Rotate={CropRectangleSetting.RotateFrames}\r\n" +
                $"Timer={PhotoSetting.SnapshotTime.TotalSeconds}\r\n" +
                $"Font={MainWindowsStyleSetting.Font}";

            File.WriteAllText(settingFile.FullName, setting);
        }
    }
}
