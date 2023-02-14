using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using Дипломная_работа___Гимаев_Амир.Classes;

namespace Дипломная_работа___Гимаев_Амир.Windows
{
    public partial class PDFFormatParameters : MetroWindow
    {

        /// <summary>
        /// Инициализация данного окна.
        /// </summary>
        /// <param name="_listOfPhotos">Экземпляр ListBox с коллекциями снимков</param>
        /// <param name="mode">Режим работы окна</param>
        public PDFFormatParameters(ListBox _listOfPhotos, string pathPDF = null)
        {
            InitializeComponent();
            PathPDFFile.Text = pathPDF;
            ScanSlider.Maximum = _listOfPhotos.Items.Count;
        }

        private void ChangeFormat(object sender, EventArgs e)
        {
            PDFClass.ChangeFormatPDF(sender as ComboBox);
        }

        private void SelectPage(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PDFClass.ChangePage(this);
        }

        private void SelectSavePathPDF(object sender, RoutedEventArgs e)
        {
            PDFClass.SelectFolderPath(PathPDFFile);
        }

        private void ParamChanged(object sender, TextChangedEventArgs e)
        {
            PDFClass.ChangePageParam(this);
        }
        private void ParamChanged(object sender, RoutedEventArgs e)
        {
            PDFClass.ChangePageParam(this);
        }

        private void SavePDFInFolder(object sender, RoutedEventArgs e)
        {
            PDFClass.SavePDFFile(PathPDFFile.Text, NamePDFFileTextBox.Text);
        }

        private void WindowIsLoaded(object sender, RoutedEventArgs e)
        {
            PDFClass.DisplayPDFPage(0);
        }

        private void SavePDFInDB(object sender, RoutedEventArgs e)
        {
            PDFClass.SavePDFFile(Environment.CurrentDirectory, "pdfDB");
            PDFClass.OpenAddDigitizedDocumentWindow();
            Close();
        }
    }
}
