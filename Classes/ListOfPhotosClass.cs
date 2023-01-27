using System.Windows.Controls;
using System.Windows.Input;

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
        /// Убирает всё выделеное в ListOfPhotos.
        /// </summary>
        public static void RemoveSelected()
        {
            foreach (ListBoxItem _photo in _listOfPhotos.Items)
                _photo.IsSelected = false;
        }
    }
}
