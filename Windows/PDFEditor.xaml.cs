using System.Windows;
using Дипломная_работа___Гимаев_Амир.Classes;

namespace Дипломная_работа___Гимаев_Амир.Windows
{
    public partial class PDFEditor : Window
    {
        private Spire.Pdf.PdfDocument pdf;

        public PDFEditor()
        {
            InitializeComponent();
        }

        // Удаляет выбранные страницы
        private void DeleteSelectedPages(object sender, RoutedEventArgs e)
        {
            PDFClass.DeleteSelectionPagesFromPDF(ref PDFViewer, PathPDFTextBox.Text, pdf);
            PDFProperty.Text = PDFClass.GetPDFProperty(pdf, PathPDFTextBox.Text);
        }

        private void OpenPDFFile(object sender, RoutedEventArgs e)
        {
            PDFClass.SelectFile(PathPDFTextBox);

            PDFClass.OpenPDF(ref PDFViewer, PathPDFTextBox.Text, out pdf);

            PDFProperty.Text = PDFClass.GetPDFProperty(pdf, PathPDFTextBox.Text);
        }
    }
}
