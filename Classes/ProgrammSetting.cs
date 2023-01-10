using ControlzEx.Theming;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    internal class ProgrammSetting
    {

        private static MainWindow MainWindowInstance; // Здесь храниться экземпляр главного окна


        static public class MainWindowsStyleSetting 
        {
            internal static string StyleTitleShade, StyleTitleColor; // Стиль окна

            internal static int SelectThemeSelectedIndex, SelectColorWindowSelectedIndex;

            internal static string Font; // Шрифт

            internal static int MainWindowHeight, MainWindowWidht;



            private static string[,] Colors = 
            {
                {"Красный", "Red"},
                {"Зелёный", "Green"},
                {"Синий", "Blue"},
                {"Пурпурный" ,"Purple"},
                {"Оранживый" ,"Orange"},
                {"Лайм" ,"Lime"},
                {"Изумрудный" ,"Emerald"},
                {"Сине-зелёный","Teal"},
                {"Бирюзовый" ,"Cyan"},
                {"Кобальтовый" ,"Cobalt"},
                {"Индиго" ,"Indigo"},
                {"Фиолетовый", "Violet"},
                {"Розовый","Pink" },
                {"Маджента", "Magenta"},
                {"Багровый" , "Crimson"},
                {"Янтарный" , "Amber"},
                {"Желтый" , "Yellow"},
                {"Коричневый" , "Brown"},
                {"Оливковый" , "Olive"},
                {"Серый" , "Steel"},
                {"Лиловый", "Mauve"},
                {"Темно-серо-коричневый", "Taupe"}
            };

            

            public static void ChangeTheme()
            {
                try { if (MainWindowInstance == null) return; } catch (Exception) {  }

                if (MainWindowInstance.DataContext != null) 
                    (MainWindowInstance.DataContext as TextBlock).FontFamily = new FontFamily(Font);


                    if (MainWindowInstance.SelectThemeComboBox.SelectedIndex == 0)
                    {
                        StyleTitleShade = ThemeManager.BaseColorLightConst;
                        MainWindowInstance.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFAFAFA"));
                        MainWindowInstance.ScanSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
                    }
                    else
                    {
                        StyleTitleShade = ThemeManager.BaseColorDarkConst;
                        MainWindowInstance.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF808080"));
                        MainWindowInstance.ScanSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6F6F6F"));
                    }


                    for (byte i = 0; i < Colors.GetLength(0); i++)
                        if (Colors[i, 0] == (MainWindowInstance.SelectColorWindowComboBox.SelectedItem as ComboBoxItem).Content.ToString())
                        { StyleTitleColor = Colors[i, 1]; break; }


                    ThemeManager.Current.ChangeTheme(MainWindowInstance, StyleTitleShade + "." + StyleTitleColor);

                WriteSettingFileNewSettings(); 

            }
        }




        static public class ScanAreaSetting 
        {
            public static int RotateFrames { get; internal set; } // Оринтация кадров

            internal static Point PositionArea;

            internal static int AreaHeight, AreaWidth;
        }




        static public class PhotoSetting 
        {
            public static string LastPathForPhotos, LastNameForPhoto, LastNameForPDF; // Последнее использованные имена

            public static TimeSpan SnapshotTime; // Время для таймера

            public static double Brightness, Contrast;

            public static int ChromaSelectedIndex;

            public static int ScanFormatSelectedIndex;

        }
        

        // Инициализцаия всех объектов 
        public static void MainWindowInitialization(MainWindow _mainWindow)
        {
            MainWindowInstance = _mainWindow;

            ScanAreaClass.MainWindowInstance = MainWindowInstance;
            ScanAreaClass.InitializationScanAreaClass();

            ListOfPhotosClass.Initialize(MainWindowInstance.ListOfPhotos);

            AForgeDocumentDisplay.Initialize(MainWindowInstance);

            MainWindowInstance.ScanArea.Height = ScanAreaSetting.AreaHeight;
            MainWindowInstance.ScanArea.Width = ScanAreaSetting.AreaWidth;

            Canvas.SetLeft(MainWindowInstance.ScanArea, ScanAreaSetting.PositionArea.X);
            Canvas.SetTop(MainWindowInstance.ScanArea, ScanAreaSetting.PositionArea.Y);

            MainWindowInstance.SelectThemeComboBox.SelectedIndex = MainWindowsStyleSetting.SelectThemeSelectedIndex;
            MainWindowInstance.SelectColorWindowComboBox.SelectedIndex = MainWindowsStyleSetting.SelectColorWindowSelectedIndex;

            MainWindowInstance.Height = MainWindowsStyleSetting.MainWindowHeight;
            MainWindowInstance.Width = MainWindowsStyleSetting.MainWindowWidht;

            MainWindowInstance.ScanBrightness.Value = PhotoSetting.Brightness;
            MainWindowInstance.ScanContrast.Value = PhotoSetting.Contrast;
            MainWindowInstance.ChromaComboBox.SelectedIndex = PhotoSetting.ChromaSelectedIndex;
            MainWindowInstance.ScanFormat.SelectedIndex = PhotoSetting.ScanFormatSelectedIndex;

            // установка шрифта ( не законченно ... )
            TextBlock _tb = new TextBlock { FontFamily = new FontFamily(MainWindowsStyleSetting.Font) };
            MainWindowInstance.DataContext = _tb;

            MainWindowInstance.ScanName.Text = PhotoSetting.LastNameForPhoto;

            MainWindowInstance.ScanPath.Text = PhotoSetting.LastPathForPhotos;

            
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
                MainWindowsStyleSetting.SelectThemeSelectedIndex = Convert.ToInt32(formatedSetting[0]);





                // Сохраненный цвет заднего фона окон инструментов
                MainWindowsStyleSetting.SelectColorWindowSelectedIndex = Convert.ToInt32(formatedSetting[1]);




                // 
                MainWindowsStyleSetting.MainWindowHeight = Convert.ToInt32(formatedSetting[2]);

                MainWindowsStyleSetting.MainWindowWidht = Convert.ToInt32(formatedSetting[3]);





                PhotoSetting.LastPathForPhotos = formatedSetting[4];

                PhotoSetting.LastNameForPhoto = formatedSetting[5];

                PhotoSetting.LastNameForPDF = formatedSetting[6];



                ScanAreaSetting.AreaHeight = Convert.ToInt32(formatedSetting[7]);

                ScanAreaSetting.AreaWidth = Convert.ToInt32(formatedSetting[8]);

                ScanAreaSetting.RotateFrames = Convert.ToInt32(formatedSetting[9]);



                PhotoSetting.SnapshotTime = new TimeSpan(0, 0, Convert.ToInt32(formatedSetting[10]));

                MainWindowsStyleSetting.Font = formatedSetting[11];

                ScanAreaSetting.PositionArea = new Point(Convert.ToInt32(formatedSetting[12].Split(';')[0]), 
                    Convert.ToInt32(formatedSetting[12].Split(';')[1]));



                PhotoSetting.Brightness = Convert.ToSingle(formatedSetting[13]); 

                PhotoSetting.Contrast = Convert.ToSingle(formatedSetting[14]);

                PhotoSetting.ChromaSelectedIndex = Convert.ToInt32(formatedSetting[15]);

                AForgeDocumentDisplay.CurrentDevice = formatedSetting[16];

                PhotoSetting.ScanFormatSelectedIndex = Convert.ToInt32(formatedSetting[17]);

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
                $"SelectThemeComboBox_SelectedIndex={MainWindowInstance.SelectThemeComboBox.SelectedIndex}\r\n" +
                $"SelectColorWindowComboBox_SelectedIndex={MainWindowInstance.SelectColorWindowComboBox.SelectedIndex}\r\n" +
                $"MetroWindowHeight={MainWindowInstance.ActualHeight}\r\n" +
                $"MetroWindowWidht={MainWindowInstance.ActualWidth}\r\n" +
                $"LastPathForPhotos={MainWindowInstance.ScanPath.Text}\r\n" +
                $"LastNameForPhoto={MainWindowInstance.ScanName.Text}\r\n" +
                $"LastNameForPDF={PhotoSetting.LastNameForPDF}\r\n" +
                $"ScanAreaHeight={MainWindowInstance.ScanArea.Height}\r\n" +
                $"ScanAreaWidth={MainWindowInstance.ScanArea.Width}\r\n" +
                $"Rotate={ScanAreaSetting.RotateFrames}\r\n" +
                $"Timer={PhotoSetting.SnapshotTime.TotalSeconds}\r\n" +
                $"Font={MainWindowsStyleSetting.Font}\r\n" +
                $"PositionScanArea={MainWindowInstance.ScanArea.TranslatePoint(new Point(0, 0), MainWindowInstance.MovingSpaceCanvas)}\r\n" +
                $"Brightness={MainWindowInstance.ScanBrightness.Value}\r\n" +
                $"Contrast={MainWindowInstance.ScanContrast.Value}\r\n" +
                $"Chroma={MainWindowInstance.ChromaComboBox.SelectedIndex}\r\n" +
                $"CurrentDeviceName={MainWindowInstance.SelectDevice.Text}\r\n" +
                $"ScanFormatSelectedIndex={MainWindowInstance.ScanFormat.SelectedIndex}";

            File.WriteAllText(settingFile.FullName, setting);
        }
    }
}
