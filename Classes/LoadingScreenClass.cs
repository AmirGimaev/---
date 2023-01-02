using System;
using System.IO;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    internal class LoadingScreenClass
    {
        public static void ReadSettingFile() // Метод для чтения всех настроек с файла Setting.txt
        {
            string CurrentDirectory = Environment.CurrentDirectory; // Текущая директория

            FileInfo SettingFile = new FileInfo(CurrentDirectory + "\\Setting.txt");

            if (SettingFile.Exists)
            {

            }
            else
            {
                SettingFile.Create();
            }
        }
    }
}
