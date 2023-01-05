using AForge.Video.DirectShow;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class AForgeDocumentDisplay
    {
        private static VideoCaptureDevice VideoFrame; // Получает изображение с устройства
        private static FilterInfoCollection Devices; // Хранит список доступных устройств

        private static string CurrentDevice; // Хранит "прозвище" текущего устройства вывода изображения

        
        public static void SelectDevice(in int index)
        {
            VideoFrame.Stop();
            VideoFrame = new VideoCaptureDevice(Devices[index].MonikerString);
            if (VideoFrame.IsRunning) VideoFrame.Start();
        }
    }
}
