using Spire.Pdf;
using System.Windows.Controls;
using Дипломная_работа___Гимаев_Амир.Windows;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class PDF
    {
        public static void SelectFolderPath(TextBox _textBox)
        {
            using (var _folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult _result = _folderBrowserDialog.ShowDialog();

                if (_result == System.Windows.Forms.DialogResult.OK && 
                    !string.IsNullOrWhiteSpace(_folderBrowserDialog.SelectedPath))
                {
                    _textBox.Text = _folderBrowserDialog.SelectedPath;
                }
            }
        }

        // Открывает окно для настройки параметров файла PDF.
        public static void OpenPDFFormatParametrsWindow(ListBox _listOfPhotos) 
            => new PDFFormatParameters(_listOfPhotos).Show();

        // Документ PDF.
        private static PdfDocument _pdfFile;


        //private static ;


        public static void ChangeFormatPDF(in ComboBox _formatComboBox)
        {
            switch (_formatComboBox.SelectedIndex) 
            {
                case 0:
                    _pdfFile = new PdfDocument(); _pdfFile.PageSettings.Size = PdfPageSize.A4;
                    break;
                case 1:
                    _pdfFile = new PdfDocument(); _pdfFile.PageSettings.Size = PdfPageSize.A3;
                    break;
                case 2:
                    _pdfFile = new PdfDocument(); _pdfFile.PageSettings.Size = PdfPageSize.A2;
                    break;
            }
        }

        public static void ChangePageParam()
        {

        }
        
    }
}
