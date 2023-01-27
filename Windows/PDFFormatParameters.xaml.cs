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
using Дипломная_работа___Гимаев_Амир.Classes;

namespace Дипломная_работа___Гимаев_Амир.Windows
{
    public partial class PDFFormatParameters : Window
    {
        public PDFFormatParameters(ListBox _listOfPhotos)
        {
            InitializeComponent();

            ScanSlider.Maximum = _listOfPhotos.Items.Count;
        }

        private void ChangeFormat(object sender, EventArgs e)
        {
            PDF.ChangeFormatPDF(sender as ComboBox);
        }

        private void SelectScan(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
    }
}
