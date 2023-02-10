using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Дипломная_работа___Гимаев_Амир.Windows
{
    /// <summary>
    /// Логика взаимодействия для PDFEditor.xaml
    /// </summary>
    public partial class PDFEditor : Window
    {
        private PdfDocument pdfDocument;

        public PDFEditor(string _pdfPath)
        {
            InitializeComponent();
            pdfDocument = new PdfDocument(_pdfPath);
        }
    }
}
