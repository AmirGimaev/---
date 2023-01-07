using System.Windows;
using Дипломная_работа___Гимаев_Амир.Classes;

namespace Дипломная_работа___Гимаев_Амир.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window
    {
        public LoadingScreen() => InitializeComponent();

        private void GetSettingForMainWindow(object sender, RoutedEventArgs e)
        {
            ProgrammSetting.ReadSettingFileAndSettingProgramm();

            if (!AForgeDocumentDisplay.CheckVideoInputDevice())
            {
                AForgeDocumentDisplay.SearchDevices.SearchDevice(sender as LoadingScreen);
            }
            else MainWindowInitializetion();
        }

        public void MainWindowInitializetion()
        {
            MainWindow _mainWindow = new MainWindow();

            _mainWindow.Show();

            Close();
        }
    }
}
