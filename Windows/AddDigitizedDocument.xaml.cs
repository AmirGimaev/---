using System.Windows;
using System.Windows.Controls;
using Дипломная_работа___Гимаев_Амир.Classes;

namespace Дипломная_работа___Гимаев_Амир.Windows
{
    public partial class AddDigitizedDocument : Window
    {
        private string fullPDFName;

        public AddDigitizedDocument()
        {
            InitializeComponent();
        }


        private void WindowIsLoaded(object sender, RoutedEventArgs e)
        {
            PDFClass.UploadDataToListOfDocumentName(ref ListOfDocumentName);
        }

        private void SelectedDocumentInList(object sender, SelectionChangedEventArgs e)
        {
            DocumentInfo.DataContext = PDFClass.SelectDocument(ref ListOfDocumentName);
        }

        private void SearchDocumentByName(object sender, TextChangedEventArgs e)
        {
            PDFClass.SearchName(ref ListOfDocumentName, (sender as TextBox).Text);
        }

        private void SavePDFFileInDataBase(object sender, RoutedEventArgs e)
        {
            PDFClass.SavePDFFileInDB((DocumentInfo.DataContext as Documents).IdCase);
        }
    }
}
