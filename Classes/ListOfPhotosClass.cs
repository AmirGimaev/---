using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class ListOfPhotosClass
    {

        private static ListBox _listOfPhotos;

        /// <summary>
        /// Инициализация элемента ListOfPhotos в данном классе (также добавляется дополнительные обработчики событий).
        /// </summary>
        /// <param name="_lb"></param>
        public static void Initialize(ListBox _lb)
        {

            _listOfPhotos = _lb;

            // Добавление обработчика события, который выделяет весь список при нажатии клавишь Ctrl + A
            _listOfPhotos.InputBindings.Add(new KeyBinding(ApplicationCommands.SelectAll,
                          new KeyGesture(Key.A, ModifierKeys.Control)));
            _listOfPhotos.CommandBindings.Add(new CommandBinding(ApplicationCommands.SelectAll, (_sender, _e) =>
            {
                _listOfPhotos.SelectAll();
            }));

        }

        /// <summary>
        /// Добавляет снимки в ListOfPhotos
        /// </summary>
        /// <param name="_listOfPhotos">Ссылка на экземпляр ListBox</param>
        /// <param name="_fullPathPhotos">Путь к графическим файлам</param>
        public static void AddPhotoToListOfPhotos(ListBox _listOfPhotos, string[] _fullPathPhotos)
        {
            foreach (string _fullPathPhoto in _fullPathPhotos)
            {
                // Если файл не является изображением, то произойдет выход из метода
                if (!_fullPathPhoto.EndsWith(".Png") & !_fullPathPhoto.EndsWith(".Jpeg")
                    & !_fullPathPhoto.EndsWith(".Tiff") & !_fullPathPhoto.EndsWith(".Bmp")) return;

                // В переменную _bi добавим ранне созданый снимок (для того, чтобы добавить этот снимок в ListOfPhotos)
                var _bi = new BitmapImage();
                _bi.BeginInit();
                _bi.UriSource = new Uri(_fullPathPhoto);
                _bi.EndInit();

                // В _sp будет храниться элемент Image (со снимком) и TextBlok (с кратким именем снимка)
                StackPanel _sp = new StackPanel() { Margin = new Thickness(5) };
                _sp.MaxHeight = 120;
                _sp.Children.Add(new System.Windows.Controls.Image() { Stretch = Stretch.Uniform, MaxHeight = 100, MaxWidth = 100 });
                _sp.Children.Add(new TextBlock());

                (_sp.Children[0] as System.Windows.Controls.Image).Source = _bi;
                (_sp.Children[1] as TextBlock).HorizontalAlignment = HorizontalAlignment.Center;
                (_sp.Children[1] as TextBlock).Text = _fullPathPhoto.Remove(0, _fullPathPhoto.LastIndexOf('\\') + 1);

                ListBoxItem _lbi = new ListBoxItem() { Content = _sp };

                _listOfPhotos.Items.Add(_lbi);
            }
        }

        /// <summary>
        /// Убирает всё выделеное в ListOfPhotos.
        /// </summary>
        public static void RemoveSelected()
        {
            foreach (ListBoxItem _photo in _listOfPhotos.Items)
                _photo.IsSelected = false;
        }

        /// <summary>
        /// Удаляет выбранные снимки.
        /// </summary>
        /// <param name="_lb">Экземпляр ListBox</param>
        public static void DeleteSelectedImages(ListBox _lb)
        {
            while (_lb.SelectedItems.Count > 0)
                _lb.Items.Remove(_lb.SelectedItem);
        }

        /// <summary>
        /// Добавляет снимки, перетаскиваемые из рабочего стола в ListOfPhotos.
        /// </summary>
        /// <param name="e">Данный параметр передает данные об переметаскиваемых файлах.</param>
        public static void DropPhotosToListOfPhotos(ref ListBox _listOfPhotos, DragEventArgs e)
        {
            string[] _files = e.Data.GetData(DataFormats.FileDrop, true) as string[];

            AddPhotoToListOfPhotos(_listOfPhotos, _files);
        }


        /// <summary>
        /// Открывает папку с снимками и выводит их в ListOfPhotos.
        /// </summary>
        /// <param name="_lb">Экземпляр ListBox.</param>
        /// <param name="_tb">Экземпляр TextBox.</param>
        public static void OpenImagesFromCatalog(ref ListBox _lb, ref TextBox _tb)
        {
            _lb.Items.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = true };
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.tiff";
            if (openFileDialog.ShowDialog() == true)
            {
                _tb.Text = openFileDialog.FileName.Remove(openFileDialog.FileName.LastIndexOf("\\"),
                    openFileDialog.FileName.Length - openFileDialog.FileName.LastIndexOf("\\"));

                foreach (string _fullPathPhoto in openFileDialog.FileNames)
                {
                    var _bi = new BitmapImage();
                    _bi.BeginInit();
                    _bi.UriSource = new Uri(_fullPathPhoto);
                    _bi.EndInit();

                    StackPanel _sp = new StackPanel() { Margin = new Thickness(5) };
                    _sp.MaxHeight = 120;
                    _sp.Children.Add(new Image() { Stretch = Stretch.Uniform, MaxHeight = 100, MaxWidth = 100 });
                    _sp.Children.Add(new TextBlock());

                    (_sp.Children[0] as Image).Source = _bi;
                    (_sp.Children[1] as TextBlock).HorizontalAlignment = HorizontalAlignment.Center;
                    (_sp.Children[1] as TextBlock).Text = _fullPathPhoto.Remove(0, _fullPathPhoto.LastIndexOf('\\') + 1);

                    ListBoxItem _lbi = new ListBoxItem() { Content = _sp };

                    _lb.Items.Add(_lbi);
                }
            }
        }

    }
}
