using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Дипломная_работа___Гимаев_Амир.Windows;
using Image = System.Windows.Controls.Image;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    /// <summary>
    /// Класс для работы с файлами формата PDF.
    /// </summary>
    static internal class PDFClass
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

        public static void SelectFile(TextBox _textBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pdf Files|*.pdf";
            if (openFileDialog.ShowDialog() == true)
            {
                _textBox.Text = openFileDialog.FileName;
            }
        }








        // --- Методы для окна PDFFormatParametrs ---





        private static Document _pdfDocument; // хранит экземпляр документа PDF.
        private static List<Thickness> pages;
        private static Rectangle pageFormat = PageSize.A4;
        private static List<string> fullPathToImages;
        private static string _pathToFiles;
        private static List<iTextSharp.text.Image> images;
        private static List<bool> saveFitImage;
        private static List<bool> isVerticalImage;

        /// <summary>
        /// Открывает окно для настройки параметров файла PDF.
        /// </summary>
        /// <param name="_listOfPhotos"></param>
        /// <param name="mode">Режим работы окна</param>
        public static void OpenPDFFormatParametrsWindow(ListBox _listOfPhotos,
            string pathToFiles, MainWindow _mw, string pathPDF = null)
        {
            if (pathToFiles == "\\")
            {
                _mw.ShowMessageAsync("Оцифровка документов", "Задайте путь к папке для сохранения снимков");
                return;
            }

            _pathToFiles = pathToFiles;

            fullPathToImages = new List<string>();
            pages = new List<Thickness>();
            images = new List<iTextSharp.text.Image>();
            saveFitImage = new List<bool>();
            isVerticalImage = new List<bool>();

            foreach (var selecteditem in _listOfPhotos.SelectedItems)
            {
                string _nameImage = (((selecteditem as ListBoxItem).Content as StackPanel).Children[1] as TextBlock).Text;
                fullPathToImages.Add(_nameImage);

                var imagePDF = iTextSharp.text.Image.GetInstance(pathToFiles + _nameImage);
                imagePDF.ScaleToFit(pageFormat);
                imagePDF.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                imagePDF.SetAbsolutePosition((PageSize.A4.Width - imagePDF.ScaledWidth) / 2,
                    (PageSize.A4.Height - imagePDF.ScaledHeight) / 2);
                pages.Add(new Thickness(0, 0, 0, 0));
                images.Add(imagePDF);
                saveFitImage.Add(true);
                isVerticalImage.Add(true);
            }

            var _pdfFormatParameters = new PDFFormatParameters(_listOfPhotos, pathPDF);

            _pdfFormatParameters.Show();
        }










        private static int CurrentPage;

        public static void ChangeFormatPDF(in ComboBox _formatComboBox)
        {
            switch (_formatComboBox.SelectedIndex)
            {
                case 0:
                    pageFormat = PageSize.A4;
                    break;
                case 1:
                    pageFormat = PageSize.A3;
                    break;
                case 2:
                    pageFormat = PageSize.A2;
                    break;
            }
        }



        public static BitmapSource DisplayPDFPage(int page)
        {
            try
            {
                using (stream = new FileStream(Environment.CurrentDirectory + "\\pdfBuilder.pdf", FileMode.Create))
                {
                    _pdfDocument = new Document(pageFormat);

                    PdfWriter.GetInstance(_pdfDocument, stream);
                    _pdfDocument.Open();
                    for (int i = 0; i < pages.Count; i++)
                    {
                        _pdfDocument.Add(images[i]);
                        if (i + 1 != pages.Count)
                        {
                            _pdfDocument.NewPage();
                        }
                    }
                    _pdfDocument.Close();
                }
                Thread.Sleep(500);
                _pdfSpireDocument = new Spire.Pdf.PdfDocument(Environment.CurrentDirectory + "\\pdfBuilder.pdf");
                return _pdfSpireDocument.SaveAsImage(page);
            }
            catch (Exception ex) { return null; }
        }


        /// <summary>
        /// Сохранение файла PDF в указаном месте.
        /// </summary>
        /// <param name="_pathSave">Путь сохранения файла.</param>
        /// <param name="_fileName">Имя файла.</param>
        public static void SavePDFFile(string _pathSave, string _fileName)
        {
            if (_pathSave == string.Empty | _fileName == string.Empty)
            {
                MessageBox.Show("Оцифровка документов", "Укажите путь сохранения и имя файла PDF.",
                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            using (var stream2 = new FileStream($"{_pathSave}\\{_fileName}.pdf",
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var _pdfDocument2 = new Document(pageFormat);

                PdfWriter.GetInstance(_pdfDocument2, stream2);
                _pdfDocument2.Open();
                for (int i = 0; i < images.Count; i++)
                {
                    _pdfDocument2.Add(images[i]);
                    if (i + 1 != images.Count) _pdfDocument2.NewPage();
                }
                _pdfDocument2.Close();
            }
        }

        static FileStream stream = null;

        /// <summary>
        /// Меняет параметры страницы
        /// </summary>
        /// <param name="_pdfFP">Экземпляр окна</param>
        public static void ChangePageParam(PDFFormatParameters _pdfFP)
        {
            try
            {
                CurrentPage = Convert.ToInt32(_pdfFP.ScanSlider.Value) - 1;
                if (_pdfFP.IsVerticalRadioButton == null | _pdfFP.SaveFitRadioButton == null) return;


                float top = Convert.ToSingle(_pdfFP.TopMediaBoxTextBox.Text);
                float bottom = Convert.ToSingle(_pdfFP.BottomMediaBoxTextBox.Text);
                float left = Convert.ToSingle(_pdfFP.LeftMediaBoxTextBox.Text);
                float right = Convert.ToSingle(_pdfFP.RightMediaBoxTextBox.Text);

                pages[CurrentPage] = new Thickness(left, top, right, bottom);

                Rectangle pageMargin = new Rectangle(0, 0, pageFormat.Width - right - left, pageFormat.Height - bottom - top);

                saveFitImage[CurrentPage] = _pdfFP.SaveFitRadioButton.IsChecked.Value;
                isVerticalImage[CurrentPage] = _pdfFP.IsVerticalRadioButton.IsChecked.Value;

                if (isVerticalImage[CurrentPage]) images[CurrentPage].Rotation = 0;
                else images[CurrentPage].Rotation = 1.57f;

                if (saveFitImage[CurrentPage]) images[CurrentPage].ScaleToFit(pageMargin);
                else images[CurrentPage].ScaleAbsolute(pageMargin);
                images[CurrentPage].SetAbsolutePosition(left, bottom);

                _pdfFP.ExampleImage.Source = DisplayPDFPage(CurrentPage);
            }

            catch { }
        }












        static private Spire.Pdf.PdfDocument _pdfSpireDocument;

        /// <summary>
        /// Перелистывание страниц.
        /// </summary>
        /// <param name="_pdfFP"></param>
        public static void ChangePage(PDFFormatParameters _pdfFP)
        {
            int _currentPage = Convert.ToInt32(_pdfFP.ScanSlider.Value) - 1;

            _pdfFP.ExampleImage.Source = DisplayPDFPage(_currentPage);

            if (_pdfFP.CurrentPageNumberTextBlock == null) return;

            _pdfFP.CurrentPageNumberTextBlock.Text = $"Страница {_pdfFP.ScanSlider.Value} из {pages.Count}";

            _pdfFP.LeftMediaBoxTextBox.Text = pages[_currentPage].Left.ToString();
            _pdfFP.RightMediaBoxTextBox.Text = pages[_currentPage].Right.ToString();
            _pdfFP.TopMediaBoxTextBox.Text = pages[_currentPage].Top.ToString();
            _pdfFP.BottomMediaBoxTextBox.Text = pages[_currentPage].Bottom.ToString();

            _pdfFP.SaveFitRadioButton.IsChecked = saveFitImage[CurrentPage];
            _pdfFP.IsVerticalRadioButton.IsChecked = isVerticalImage[CurrentPage];
        }








        // --- Методы для редактирования PDF ---

        /// <summary>
        /// Метод открывающий окно для редактирования PDF файла.
        /// </summary>
        /// <param name="_pdfPath"></param>
        public static void OpenPDFEditor()
            => new PDFEditor().Show();


        /// <summary>
        /// Метод для открытия файла PDF.
        /// </summary>
        /// <param name="_pdfViewer">Ссылка на экземпляр PDFViewer.</param>
        /// <param name="_pathPDF">Путь к файлу PDF.</param>
        public static void OpenPDF(ref ListBox _pdfViewer, string _pathPDF, out Spire.Pdf.PdfDocument _pdfSpireDocument)
        {
            _pdfViewer.Items.Clear();

            _pdfSpireDocument = new Spire.Pdf.PdfDocument(_pathPDF);

            for (int i = 0; i < _pdfSpireDocument.Pages.Count; i++)
            {
                Image _image = new Image();
                _image.Height = 300;
                _image.Source = _pdfSpireDocument.SaveAsImage(i);
                _image.Name = "page_" + i.ToString();
                _pdfViewer.Items.Add(_image);
            }
        }


        /// <summary>
        /// Метод для безвозвратного удаления выбранных в PDFViewer страниц.
        /// </summary>
        /// <param name="_pdfViewer"></param>
        /// <param name="_pathPDF"></param>
        public static void DeleteSelectionPagesFromPDF(ref ListBox _pdfViewer, string _pathPDF, Spire.Pdf.PdfDocument pdf)
        {
            try
            {
                foreach (var item in _pdfViewer.SelectedItems)
                {
                    Image _image = item as Image;
                    pdf.Pages.RemoveAt(Convert.ToInt32(_image.Name.Remove(0, 5)));
                    _pdfViewer.Items.Remove(item);
                }
            }
            catch { }
            pdf.SaveToFile(_pathPDF);
        }

        /// <summary>
        /// Возрващает свойтсва документа PDF.
        /// </summary>
        /// <param name="pdf">PDF файл.</param>
        /// <returns>Свойство файла.</returns>
        public static string GetPDFProperty(Spire.Pdf.PdfDocument pdf, string _pathFile)
        {
            string _property =
                $"Имя - {pdf.DocumentInformation.Title}\n\n" +
                $"Автор - {pdf.DocumentInformation.Author}\n\n" +
                $"Дата создания - {pdf.DocumentInformation.CreationDate}\n\n" +
                $"Дата изменения - {pdf.DocumentInformation.ModificationDate}\n\n" +
                $"Версия PDF - {pdf.FileInfo.Version}\n\n" +
                $"Количество страниц - {pdf.Pages.Count}\n\n" +
                $"Размер страниц - {pdf.PageSettings.Size.Width}x{pdf.PageSettings.Size.Height}\n\n" +
                $"Размер файла - {new FileInfo(_pathFile).Length / 1024} Кбайт";

            return _property;
        }








        /*  Методы для окна добавления оцифрованного документа а базу данных */

        /// <summary>
        /// Открывает окно для добавления оцифрованного документа в БД.
        /// </summary>
        /// <param name="fullPDFName">Полный путь к файлу PDF.</param>
        public static void OpenAddDigitizedDocumentWindow()
        {
            AddDigitizedDocument _addDigitizedDocument = new AddDigitizedDocument();
            _addDigitizedDocument.Show();
        }

        /// <summary>
        /// Обновление списка документов.
        /// </summary>
        /// <param name="_lb">Экземпляр ListBox.</param>
        public static void UploadDataToListOfDocumentName(ref ListBox _lb)
        {
            DigitalDocumentsDataBase db = new DigitalDocumentsDataBase();

            _lb.ItemsSource = db.Documents.ToList();
        }

        /// <summary>
        /// Поиск документа по слову.
        /// </summary>
        /// <param name="_lb">Экземпляр ListBox.</param>
        /// <param name="searchWord">Слово, по которой будет выполняться поиск.</param>
        public static void SearchName(ref ListBox _lb, string searchWord)
        {
            DigitalDocumentsDataBase db = new DigitalDocumentsDataBase();

            var result = db.Documents.Where(w => w.Name.Contains(searchWord)).ToList();

            _lb.ItemsSource = result;
        }

        public static Documents SelectDocument(ref ListBox _lb)
        {
            return _lb.SelectedItem as Documents;
        }


        /// <summary>
        /// Добавляет запись и сохраняет файл PDF в БД
        /// </summary>
        /// <param name="idCaseDocument">ID документа</param>
        /// <param name="fullPDFName">Полный путь файла PDF</param>
        public static void SavePDFFileInDB(int idCaseDocument)
        {
            Digitized _digitizedPDF = new Digitized();

            _digitizedPDF.IdCase = idCaseDocument;
            _digitizedPDF.DateOfDigitization = DateTime.Now;
            string path = Environment.CurrentDirectory + "\\" + "pdfDB.pdf";
            bool p = File.Exists("C:\\Users\\Amir\\Desktop\\p_0.Jpeg");
            _digitizedPDF.DigitizedFile = File.ReadAllBytes(path);

            DigitalDocumentsDataBase db = new DigitalDocumentsDataBase();

            db.Digitized.Add(_digitizedPDF);
            db.SaveChanges();
        }
    }
}
