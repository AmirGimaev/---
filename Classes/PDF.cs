using System.Windows.Forms;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class PDF
    {
        public static void SelectFolderPath(System.Windows.Controls.TextBox _textBox)
        {
            using (var _folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult _result = _folderBrowserDialog.ShowDialog();

                if (_result == DialogResult.OK && !string.IsNullOrWhiteSpace(_folderBrowserDialog.SelectedPath))
                {
                    _textBox.Text = _folderBrowserDialog.SelectedPath;
                }
            }
        }
    }
}
