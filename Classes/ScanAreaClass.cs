using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class ScanAreaClass
    {
        public static MainWindow MainWindowInstance;

        private static Canvas _canvas;

        private static Grid _scanArea;

        private static Point _oldPos, _newPos; // Данные переменные хранят старые и новые координаты мыши в относительно элемента MovingSpace_Canvas

        private static double _old_width, _old_height; // Данные переменные хранят старые значения ширины и высоты элемента ScanArea

        private static Vector _movePoint; // Здесь хранится позиция мыщи относительно элемента ScanArea

        private static Vector _difference1, _difference2;



        public static void InitializationScanAreaClass()
        {
            _canvas = MainWindowInstance.MovingSpaceCanvas;
            _scanArea = MainWindowInstance.ScanArea;

            _canvas.MouseMove += CheckScanAreaSize; // благодаря этому ScanArea не будет выходит за рамки MovingSpaceCanvas
        }

        private static void FindCoordinatesAndOldSize(MouseButtonEventArgs eventArgs)
        {
            _old_width = _scanArea.Width; _old_height = _scanArea.Height;
            _oldPos = Mouse.GetPosition(_canvas);

            _movePoint = (Vector)eventArgs.GetPosition(_scanArea);
            _scanArea.CaptureMouse();
        }


        // RemoveAllCanvasMouseEventArgs используется для очищения всех обработчиков событий MouseMove
        public static void RemoveAllCanvasMouseEventArgs()
        {
            _canvas.MouseMove -= ScanAreaLeftTopChangeSize;
            _canvas.MouseMove -= ScanAreaLeftBottonChangeSize;
            _canvas.MouseMove -= ScanAreaRightTopChangeSize;
            _canvas.MouseMove -= ScanAreaRightBottonChangeSize;

            _canvas.MouseMove -= ScanAreaTopChangeSize;
            _canvas.MouseMove -= ScanAreaBottonChangeSize;
            _canvas.MouseMove -= ScanAreaLeftChangeSize;
            _canvas.MouseMove -= ScanAreaRightChangeSize;

            _canvas.MouseMove -= ScanAreaDragMode;

            _scanArea.ReleaseMouseCapture();

            MainWindowInstance.Cursor = Cursors.Arrow;
        }

        

        private static void CheckScanAreaSize(object sender, MouseEventArgs e) // Ограничение размера CropSize
        {
            _difference1 = (Vector)_scanArea.TranslatePoint(new Point(0, 0),_canvas);
            _difference2 = new Vector (_canvas.ActualWidth, _canvas.ActualHeight) 
                - (Vector)_scanArea.TranslatePoint(new Point(_scanArea.ActualWidth, _scanArea.ActualHeight), _canvas) ;
        }



        // Следующие 16 методов реализуют функцию изменения размера ScanArea
        public static void ScanAreaLeftTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaLeftTopChangeSize;
        }
        private static void ScanAreaLeftTopChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;
                _newPos = Mouse.GetPosition(_canvas);

                var p = e.GetPosition(_canvas) - _movePoint;

                if (_old_width + (_oldPos.X - _newPos.X) > 100)
                { _scanArea.Width = _old_width + (_oldPos.X - _newPos.X); Canvas.SetLeft(_scanArea, p.X); }

                if (_old_height + (_oldPos.Y - _newPos.Y) > 100)
                { _scanArea.Height = _old_height + (_oldPos.Y - _newPos.Y); Canvas.SetTop(_scanArea, p.Y); }
                return;
            }
        }

        public static void ScanAreaRightTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaRightTopChangeSize;
        }
        private static void ScanAreaRightTopChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;

                _newPos = Mouse.GetPosition(_canvas);

                var p = e.GetPosition(_canvas) - (_movePoint + new Vector(_scanArea.ActualWidth, 0));

                if (_old_width - (_oldPos.X - _newPos.X) > 100)
                { _scanArea.Width = _old_width - (_oldPos.X - _newPos.X); Canvas.SetLeft(_scanArea, p.X + _old_width); }

                if (_old_height + (_oldPos.Y - _newPos.Y) > 100)
                { _scanArea.Height = _old_height + (_oldPos.Y - _newPos.Y); Canvas.SetTop(_scanArea, p.Y); }
                return;
            }
        }

        public static void ScanAreaRightBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaRightBottonChangeSize;
        }
        private static void ScanAreaRightBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;
                _newPos = Mouse.GetPosition(_canvas);

                var p = e.GetPosition(_canvas) - (_movePoint + new Vector(_scanArea.Width, _scanArea.Height));

                if (_old_width - (_oldPos.X - _newPos.X) > 100)
                { _scanArea.Width = _old_width - (_oldPos.X - _newPos.X); Canvas.SetLeft(_scanArea, p.X + _old_width); }

                if (_old_height - (_oldPos.Y - _newPos.Y) > 100)
                { _scanArea.Height = _old_height - (_oldPos.Y - _newPos.Y); Canvas.SetTop(_scanArea, p.Y + _old_height); }
                return;
            }
        }

        public static void ScanAreaLeftBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaLeftBottonChangeSize;
        }
        private static void ScanAreaLeftBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;
                _newPos = Mouse.GetPosition(_canvas);

                var p = e.GetPosition(_canvas) - (_movePoint + new Vector(0, _scanArea.Height));

                if (_old_width + (_oldPos.X - _newPos.X) > 100)
                { _scanArea.Width = _old_width + (_oldPos.X - _newPos.X); Canvas.SetLeft(_scanArea, p.X); }

                if (_old_height - (_oldPos.Y - _newPos.Y) > 100)
                { _scanArea.Height = _old_height - (_oldPos.Y - _newPos.Y); Canvas.SetTop(_scanArea, p.Y + _old_height); }
                return;
            }
        }



        public static void ScanAreaTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaTopChangeSize;
        }
        private static void ScanAreaTopChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNS;
                _newPos = Mouse.GetPosition(_canvas);

                var p = e.GetPosition(_canvas) - _movePoint;

                if (_old_height + (_oldPos.Y - _newPos.Y) > 100)
                { _scanArea.Height = _old_height + (_oldPos.Y - _newPos.Y); Canvas.SetTop(_scanArea, p.Y); }
                return;
            }
        }

        public static void ScanAreaBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaBottonChangeSize;
        }
        private static void ScanAreaBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNS;

                _newPos = Mouse.GetPosition(_canvas);

                if (_old_height - (_oldPos.Y - _newPos.Y) > 100)
                { _scanArea.Height = _old_height - (_oldPos.Y - _newPos.Y); }
                return;
            }
        }

        public static void ScanAreaRightLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaRightChangeSize;
        }
        private static void ScanAreaRightChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeWE;

                _newPos = Mouse.GetPosition(_canvas);

                var p = e.GetPosition(_canvas) - (_movePoint + new Vector(_scanArea.ActualWidth, 0));

                if (_old_width - (_oldPos.X - _newPos.X) > 100)
                { _scanArea.Width = _old_width - (_oldPos.X - _newPos.X); Canvas.SetLeft(_scanArea, p.X + _old_width); }
                return;
            }
        }

        public static void ScanAreaLeftLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _canvas.MouseMove += ScanAreaLeftChangeSize;
        }
        private static void ScanAreaLeftChangeSize(object sender, MouseEventArgs e)
        {
            if (_difference1.X >= 1 & _difference2.X >= 1 & _difference1.Y >= 1 & _difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeWE;

                _newPos = Mouse.GetPosition(_canvas);

                var p = e.GetPosition(_canvas) - _movePoint;

                if (_old_width + (_oldPos.X - _newPos.X) > 100)
                { _scanArea.Width = _old_width + (_oldPos.X - _newPos.X); Canvas.SetLeft(_scanArea, p.X); }
                return;
            }
        }



        // Метод ScanAreaDragMode реализует функцию перемещения ScanArea.
        public static void ScanAreaCenterLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            _oldPos = Mouse.GetPosition(_scanArea);

            _canvas.MouseMove += ScanAreaDragMode;
        }
        private static void ScanAreaDragMode(object sender, MouseEventArgs e)
        {
             if (_difference1.X < 0) { Canvas.SetLeft(_scanArea, 0); RemoveAllCanvasMouseEventArgs(); return; }
             if (_difference2.X < 0) { Canvas.SetLeft(_scanArea, _canvas.ActualWidth - _scanArea.ActualWidth); RemoveAllCanvasMouseEventArgs(); return; }
             if (_difference1.Y < 0) { Canvas.SetTop(_scanArea, 0); RemoveAllCanvasMouseEventArgs(); return; }
             if (_difference2.Y < 0) { Canvas.SetTop(_scanArea, _canvas.ActualHeight - _scanArea.ActualHeight); RemoveAllCanvasMouseEventArgs(); return; }

             Canvas.SetLeft(_scanArea, e.GetPosition(_canvas).X - _oldPos.X);
             Canvas.SetTop(_scanArea, e.GetPosition(_canvas).Y - _oldPos.Y);
        }


        // !!!!!!!!!!!!!!! надо завершить !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public static void ScanAreaAdaptSize(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Vector difference = (Point)e.NewSize - (Point)e.PreviousSize;
                Vector XY_left_top_cropRect = _scanArea.PointToScreen(new Point(0, 0)) - _canvas.PointToScreen(new Point(0, 0));
                Vector XY_rigth_bottom_cropRect = _canvas.PointToScreen(new Point(0, 0)) + new Vector(_canvas.ActualWidth, _canvas.ActualHeight)
                    - (_scanArea.PointToScreen(new Point(0, 0)) + new Vector(_scanArea.ActualWidth - 5, _scanArea.ActualHeight - 5));

                if (XY_rigth_bottom_cropRect.X - 5 < 0) Canvas.SetLeft(_scanArea, e.NewSize.Width - _scanArea.ActualWidth);
                if (XY_rigth_bottom_cropRect.Y - 5 < 0) Canvas.SetTop(_scanArea, e.NewSize.Height - _scanArea.ActualHeight);
            }
            //if (XY_left_top_cropRect.X - 5 < 0) Canvas.SetLeft(cropRect, 5);
            //if (XY_left_top_cropRect.Y - 5 < 0) Canvas.SetTop(cropRect, 5);
            catch (System.NullReferenceException ) { }
        }
    }
}
