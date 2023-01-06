using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class ScanAreaClass
    {
        public static MainWindow MainWindowInstance;

        private static Canvas canvas;

        private static Grid scanArea;

        private static Point oldPos, newPos; // Данные переменные хранят старые и новые координаты мыши в относительно элемента MovingSpace_Canvas

        private static double old_width, old_height; // Данные переменные хранят старые значения ширины и высоты элемента ScanArea

        private static Vector _movePoint; // Здесь хранится позиция мыщи относительно элемента ScanArea

        private static Vector difference1, difference2;



        public static void InitializationScanAreaClass()
        {
            canvas = MainWindowInstance.MovingSpaceCanvas;
            scanArea = canvas.Children[1] as Grid;

            canvas.MouseMove += CheckScanAreaSize; // благодаря этому ScanArea не будет выходит за рамки MovingSpaceCanvas
        }

        private static void FindCoordinatesAndOldSize(MouseButtonEventArgs eventArgs)
        {
            old_width = scanArea.Width; old_height = scanArea.Height;
            oldPos = Mouse.GetPosition(canvas);

            _movePoint = (Vector)eventArgs.GetPosition(scanArea);
            scanArea.CaptureMouse();
        }

        // RemoveAllCanvasMouseEventArgs используется для очищения всех обработчиков событий MouseMove
        public static void RemoveAllCanvasMouseEventArgs()
        {
            canvas.MouseMove -= ScanAreaLeftTopChangeSize;
            canvas.MouseMove -= ScanAreaLeftBottonChangeSize;
            canvas.MouseMove -= ScanAreaRightTopChangeSize;
            canvas.MouseMove -= ScanAreaRightBottonChangeSize;

            canvas.MouseMove -= ScanAreaTopChangeSize;
            canvas.MouseMove -= ScanAreaBottonChangeSize;
            canvas.MouseMove -= ScanAreaLeftChangeSize;
            canvas.MouseMove -= ScanAreaRightChangeSize;

            canvas.MouseMove -= ScanAreaDragMode;

            scanArea.ReleaseMouseCapture();

            MainWindowInstance.Cursor = Cursors.Arrow;
        }

        

        private static void CheckScanAreaSize(object sender, MouseEventArgs e) // Ограничение размера CropSize
        {
            difference1 = (Vector)scanArea.TranslatePoint(new Point(0, 0),canvas);
            difference2 = new Vector (canvas.ActualWidth, canvas.ActualHeight) 
                - (Vector)scanArea.TranslatePoint(new Point(scanArea.ActualWidth, scanArea.ActualHeight), canvas) ;
        }



        // Следующие 16 методов реализуют функцию изменения размера ScanArea
        public static void ScanAreaLeftTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaLeftTopChangeSize;
        }
        private static void ScanAreaLeftTopChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - _movePoint;

                if (old_width + (oldPos.X - newPos.X) > 100)
                { scanArea.Width = old_width + (oldPos.X - newPos.X); Canvas.SetLeft(scanArea, p.X); }

                if (old_height + (oldPos.Y - newPos.Y) > 100)
                { scanArea.Height = old_height + (oldPos.Y - newPos.Y); Canvas.SetTop(scanArea, p.Y); }
                return;
            }
        }

        public static void ScanAreaRightTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaRightTopChangeSize;
        }
        private static void ScanAreaRightTopChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;

                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(scanArea.ActualWidth, 0));

                if (old_width - (oldPos.X - newPos.X) > 100)
                { scanArea.Width = old_width - (oldPos.X - newPos.X); Canvas.SetLeft(scanArea, p.X + old_width); }

                if (old_height + (oldPos.Y - newPos.Y) > 100)
                { scanArea.Height = old_height + (oldPos.Y - newPos.Y); Canvas.SetTop(scanArea, p.Y); }
                return;
            }
        }

        public static void ScanAreaRightBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaRightBottonChangeSize;
        }
        private static void ScanAreaRightBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(scanArea.Width, scanArea.Height));

                if (old_width - (oldPos.X - newPos.X) > 100)
                { scanArea.Width = old_width - (oldPos.X - newPos.X); Canvas.SetLeft(scanArea, p.X + old_width); }

                if (old_height - (oldPos.Y - newPos.Y) > 100)
                { scanArea.Height = old_height - (oldPos.Y - newPos.Y); Canvas.SetTop(scanArea, p.Y + old_height); }
                return;
            }
        }

        public static void ScanAreaLeftBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaLeftBottonChangeSize;
        }
        private static void ScanAreaLeftBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNWSE;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(0, scanArea.Height));

                if (old_width + (oldPos.X - newPos.X) > 100)
                { scanArea.Width = old_width + (oldPos.X - newPos.X); Canvas.SetLeft(scanArea, p.X); }

                if (old_height - (oldPos.Y - newPos.Y) > 100)
                { scanArea.Height = old_height - (oldPos.Y - newPos.Y); Canvas.SetTop(scanArea, p.Y + old_height); }
                return;
            }
        }



        public static void ScanAreaTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaTopChangeSize;
        }
        private static void ScanAreaTopChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNS;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - _movePoint;

                if (old_height + (oldPos.Y - newPos.Y) > 100)
                { scanArea.Height = old_height + (oldPos.Y - newPos.Y); Canvas.SetTop(scanArea, p.Y); }
                return;
            }
        }

        public static void ScanAreaBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaBottonChangeSize;
        }
        private static void ScanAreaBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeNS;

                newPos = Mouse.GetPosition(canvas);

                if (old_height - (oldPos.Y - newPos.Y) > 100)
                { scanArea.Height = old_height - (oldPos.Y - newPos.Y); }
                return;
            }
        }

        public static void ScanAreaRightLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaRightChangeSize;
        }
        private static void ScanAreaRightChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeWE;

                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(scanArea.ActualWidth, 0));

                if (old_width - (oldPos.X - newPos.X) > 100)
                { scanArea.Width = old_width - (oldPos.X - newPos.X); Canvas.SetLeft(scanArea, p.X + old_width); }
                return;
            }
        }

        public static void ScanAreaLeftLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += ScanAreaLeftChangeSize;
        }
        private static void ScanAreaLeftChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                MainWindowInstance.Cursor = Cursors.SizeWE;

                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - _movePoint;

                if (old_width + (oldPos.X - newPos.X) > 100)
                { scanArea.Width = old_width + (oldPos.X - newPos.X); Canvas.SetLeft(scanArea, p.X); }
                return;
            }
        }



        // Метод ScanAreaDragMode реализует функцию перемещения ScanArea.
        public static void ScanAreaCenterLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            oldPos = Mouse.GetPosition(scanArea);

            canvas.MouseMove += ScanAreaDragMode;
        }
        private static void ScanAreaDragMode(object sender, MouseEventArgs e)
        {
             if (difference1.X < 0) { Canvas.SetLeft(scanArea, 0); RemoveAllCanvasMouseEventArgs(); return; }
             if (difference2.X < 0) { Canvas.SetLeft(scanArea, canvas.ActualWidth - scanArea.ActualWidth); RemoveAllCanvasMouseEventArgs(); return; }
             if (difference1.Y < 0) { Canvas.SetTop(scanArea, 0); RemoveAllCanvasMouseEventArgs(); return; }
             if (difference2.Y < 0) { Canvas.SetTop(scanArea, canvas.ActualHeight - scanArea.ActualHeight); RemoveAllCanvasMouseEventArgs(); return; }

             Canvas.SetLeft(scanArea, e.GetPosition(canvas).X - oldPos.X);
             Canvas.SetTop(scanArea, e.GetPosition(canvas).Y - oldPos.Y);
        }


        // !!!!!!!!!!!!!!! надо завершить !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public static void ScanAreaAdaptSize(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Vector difference = (Point)e.NewSize - (Point)e.PreviousSize;
                Vector XY_left_top_cropRect = scanArea.PointToScreen(new Point(0, 0)) - canvas.PointToScreen(new Point(0, 0));
                Vector XY_rigth_bottom_cropRect = canvas.PointToScreen(new Point(0, 0)) + new Vector(canvas.ActualWidth, canvas.ActualHeight)
                    - (scanArea.PointToScreen(new Point(0, 0)) + new Vector(scanArea.ActualWidth - 5, scanArea.ActualHeight - 5));

                if (XY_rigth_bottom_cropRect.X - 5 < 0) Canvas.SetLeft(scanArea, e.NewSize.Width - scanArea.ActualWidth);
                if (XY_rigth_bottom_cropRect.Y - 5 < 0) Canvas.SetTop(scanArea, e.NewSize.Height - scanArea.ActualHeight);
            }
            //if (XY_left_top_cropRect.X - 5 < 0) Canvas.SetLeft(cropRect, 5);
            //if (XY_left_top_cropRect.Y - 5 < 0) Canvas.SetTop(cropRect, 5);
            catch (System.NullReferenceException ) { }
        }
    }
}
