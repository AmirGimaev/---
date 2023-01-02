using ControlzEx.Standard;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Дипломная_работа___Гимаев_Амир.Classes
{
    static internal class CropRectangleClass
    {
        public static MainWindow mainWindow;

        private static Canvas canvas;

        private static Grid cropRect;

        private static Point oldPos, newPos; // Данные переменные хранят старые и новые координаты мыши в относительно элемента MovingSpace_Canvas
        private static double old_width, old_height; // Данные переменные хранят старые значения ширины и высоты элемента CropRectangle

        private static Vector _movePoint; // Здесь хранится позиция мыщи относительно элемента CropRectangle


        private static Vector difference1, difference2;


        public static void InitializationCropRectangleClass()
        {
            canvas = (mainWindow.Content as Grid).FindName("MovingSpaceCanvas") as Canvas;
            cropRect = canvas.Children[1] as Grid;

            canvas.MouseMove += CheckCropRectangleSize; // благодаря этому CropRectangle не будет выходит за рамки MovingSpaceCanvas
        }

        private static void FindCoordinatesAndOldSize(MouseButtonEventArgs eventArgs)
        {
            old_width = cropRect.Width; old_height = cropRect.Height;
            oldPos = Mouse.GetPosition(canvas);

            _movePoint = (Vector)eventArgs.GetPosition(cropRect);
            cropRect.CaptureMouse();
        }

        // RemoveAllCanvasMouseEventArgs используется для очищения всех обработчиков событий MouseMove
        public static void RemoveAllCanvasMouseEventArgs()
        {
            canvas.MouseMove -= CropRectangleLeftTopChangeSize;
            canvas.MouseMove -= CropRectangleLeftBottonChangeSize;
            canvas.MouseMove -= CropRectangleRightTopChangeSize;
            canvas.MouseMove -= CropRectangleRightBottonChangeSize;

            canvas.MouseMove -= CropRectangleTopChangeSize;
            canvas.MouseMove -= CropRectangleBottonChangeSize;
            canvas.MouseMove -= CropRectangleLeftChangeSize;
            canvas.MouseMove -= CropRectangleRightChangeSize;

            canvas.MouseMove -= CropRectangleDragMode;

            cropRect.ReleaseMouseCapture();

            mainWindow.Cursor = Cursors.Arrow;
        }

        

        private static void CheckCropRectangleSize(object sender, MouseEventArgs e) // Ограничение размера CropSize
        {
            difference1 = (Vector)cropRect.TranslatePoint(new Point(0, 0),canvas);
            difference2 = new Vector (canvas.ActualWidth, canvas.ActualHeight) 
                - (Vector)cropRect.TranslatePoint(new Point(cropRect.ActualWidth, cropRect.ActualHeight), canvas) ;
        }



        // Следующие 16 методов реализуют функцию изменения размера CropRectangle
        public static void CropRectangleLeftTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleLeftTopChangeSize;
        }
        private static void CropRectangleLeftTopChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeNWSE;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - _movePoint;

                if (old_width + (oldPos.X - newPos.X) > 100)
                { cropRect.Width = old_width + (oldPos.X - newPos.X); Canvas.SetLeft(cropRect, p.X); }

                if (old_height + (oldPos.Y - newPos.Y) > 100)
                { cropRect.Height = old_height + (oldPos.Y - newPos.Y); Canvas.SetTop(cropRect, p.Y); }
                return;
            }
        }

        public static void CropRectangleRightTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleRightTopChangeSize;
        }
        private static void CropRectangleRightTopChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeNWSE;

                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(cropRect.ActualWidth, 0));

                if (old_width - (oldPos.X - newPos.X) > 100)
                { cropRect.Width = old_width - (oldPos.X - newPos.X); Canvas.SetLeft(cropRect, p.X + old_width); }

                if (old_height + (oldPos.Y - newPos.Y) > 100)
                { cropRect.Height = old_height + (oldPos.Y - newPos.Y); Canvas.SetTop(cropRect, p.Y); }
                return;
            }
        }

        public static void CropRectangleRightBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleRightBottonChangeSize;
        }
        private static void CropRectangleRightBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeNWSE;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(cropRect.Width, cropRect.Height));

                if (old_width - (oldPos.X - newPos.X) > 100)
                { cropRect.Width = old_width - (oldPos.X - newPos.X); Canvas.SetLeft(cropRect, p.X + old_width); }

                if (old_height - (oldPos.Y - newPos.Y) > 100)
                { cropRect.Height = old_height - (oldPos.Y - newPos.Y); Canvas.SetTop(cropRect, p.Y + old_height); }
                return;
            }
        }

        public static void CropRectangleLeftBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleLeftBottonChangeSize;
        }
        private static void CropRectangleLeftBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeNWSE;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(0, cropRect.Height));

                if (old_width + (oldPos.X - newPos.X) > 100)
                { cropRect.Width = old_width + (oldPos.X - newPos.X); Canvas.SetLeft(cropRect, p.X); }

                if (old_height - (oldPos.Y - newPos.Y) > 100)
                { cropRect.Height = old_height - (oldPos.Y - newPos.Y); Canvas.SetTop(cropRect, p.Y + old_height); }
                return;
            }
        }



        public static void CropRectangleTopLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleTopChangeSize;
        }
        private static void CropRectangleTopChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeNS;
                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - _movePoint;

                if (old_height + (oldPos.Y - newPos.Y) > 100)
                { cropRect.Height = old_height + (oldPos.Y - newPos.Y); Canvas.SetTop(cropRect, p.Y); }
                return;
            }
        }

        public static void CropRectangleBottonLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleBottonChangeSize;
        }
        private static void CropRectangleBottonChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeNS;

                newPos = Mouse.GetPosition(canvas);

                if (old_height - (oldPos.Y - newPos.Y) > 100)
                { cropRect.Height = old_height - (oldPos.Y - newPos.Y); }
                return;
            }
        }

        public static void CropRectangleRightLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleRightChangeSize;
        }
        private static void CropRectangleRightChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeWE;

                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - (_movePoint + new Vector(cropRect.ActualWidth, 0));

                if (old_width - (oldPos.X - newPos.X) > 100)
                { cropRect.Width = old_width - (oldPos.X - newPos.X); Canvas.SetLeft(cropRect, p.X + old_width); }
                return;
            }
        }

        public static void CropRectangleLeftLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            canvas.MouseMove += CropRectangleLeftChangeSize;
        }
        private static void CropRectangleLeftChangeSize(object sender, MouseEventArgs e)
        {
            if (difference1.X >= 1 & difference2.X >= 1 & difference1.Y >= 1 & difference2.Y >= 1)
            {
                mainWindow.Cursor = Cursors.SizeWE;

                newPos = Mouse.GetPosition(canvas);

                var p = e.GetPosition(canvas) - _movePoint;

                if (old_width + (oldPos.X - newPos.X) > 100)
                { cropRect.Width = old_width + (oldPos.X - newPos.X); Canvas.SetLeft(cropRect, p.X); }
                return;
            }
        }



        // Метод CropRectangleDragMode реализует функцию перемещения CropRectangle.
        public static void CropRectangleCenterLBM(object sender, MouseButtonEventArgs eventArgs)
        {
            FindCoordinatesAndOldSize(eventArgs);

            RemoveAllCanvasMouseEventArgs();

            oldPos = Mouse.GetPosition(cropRect);

            canvas.MouseMove += CropRectangleDragMode;
        }
        private static void CropRectangleDragMode(object sender, MouseEventArgs e)
        {
             if (difference1.X < 0) { Canvas.SetLeft(cropRect, 0); RemoveAllCanvasMouseEventArgs(); return; }
             if (difference2.X < 0) { Canvas.SetLeft(cropRect, canvas.ActualWidth - cropRect.ActualWidth); RemoveAllCanvasMouseEventArgs(); return; }
             if (difference1.Y < 0) { Canvas.SetTop(cropRect, 0); RemoveAllCanvasMouseEventArgs(); return; }
             if (difference2.Y < 0) { Canvas.SetTop(cropRect, canvas.ActualHeight - cropRect.ActualHeight); RemoveAllCanvasMouseEventArgs(); return; }

             Canvas.SetLeft(cropRect, e.GetPosition(canvas).X - oldPos.X);
             Canvas.SetTop(cropRect, e.GetPosition(canvas).Y - oldPos.Y);
        }



        public static void CropRectangleAdaptSize(object sender, SizeChangedEventArgs e)
        {
            Vector difference = (Point)e.NewSize - (Point)e.PreviousSize;
            Vector XY_left_top_cropRect = cropRect.PointToScreen(new Point(0, 0)) - canvas.PointToScreen(new Point(0, 0));
            Vector XY_rigth_bottom_cropRect = canvas.PointToScreen(new Point(0, 0)) + new Vector(canvas.ActualWidth, canvas.ActualHeight)
                - (cropRect.PointToScreen(new Point(0, 0)) + new Vector(cropRect.ActualWidth - 5, cropRect.ActualHeight - 5));

            if (XY_rigth_bottom_cropRect.X - 5 < 0) Canvas.SetLeft(cropRect, e.NewSize.Width - cropRect.ActualWidth);
            if (XY_rigth_bottom_cropRect.Y - 5 < 0) Canvas.SetTop(cropRect, e.NewSize.Height - cropRect.ActualHeight);
            //if (XY_left_top_cropRect.X - 5 < 0) Canvas.SetLeft(cropRect, 5);
            //if (XY_left_top_cropRect.Y - 5 < 0) Canvas.SetTop(cropRect, 5);
        }
    }
}
