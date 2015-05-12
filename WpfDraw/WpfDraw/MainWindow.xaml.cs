using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.Diagnostics;

namespace WpfDraw
{
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty DraggedProperty =
            DependencyProperty.RegisterAttached("Dragged", typeof(bool), typeof(MainWindow),
            new PropertyMetadata(false));

        public static void SetDragged(DependencyObject target, bool value)
        {
            target.SetValue(DraggedProperty, value);
        }
        public static bool GetDragged(DependencyObject target)
        {
            return (bool) target.GetValue(DraggedProperty);
        }


        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.RegisterAttached("StartPoint", 
            typeof(Point), 
            typeof(MainWindow), 
            new UIPropertyMetadata(new Point()));

        public static Point GetStartPoint(DependencyObject obj)
        {
            return (Point)obj.GetValue(StartPointProperty);
        }

        public static void SetStartPoint(DependencyObject obj, Point value)
        {
            obj.SetValue(StartPointProperty, value);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ellipse elipse = new Ellipse();
            elipse.Fill = Brushes.Aqua;
            elipse.Width = 100;
            elipse.Height = 100;
            elipse.PreviewMouseDown += new MouseButtonEventHandler(elipse_MouseDown);
            elipse.PreviewMouseMove += new MouseEventHandler(elipse_MouseMove);
            elipse.PreviewMouseUp += new MouseButtonEventHandler(elipse_MouseUp);
            canvas.Children.Add(elipse);
        }

        void elipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UIElement obj = sender as UIElement;
            if (obj == null || !GetDragged(obj))
            {
                return;
            }
            e.Handled = true;
            SetDragged(sender as DependencyObject, false);
        }

        void elipse_MouseMove(object sender, MouseEventArgs e)
        {
            UIElement obj = sender as UIElement;
            if (obj == null || !GetDragged(obj))
            {
                return;
            }

            e.Handled = true;
            Point start = GetStartPoint(obj);
            Canvas.SetTop(obj, e.GetPosition(canvas).Y - start.Y);
            Canvas.SetLeft(obj, e.GetPosition(canvas).X - start.X);
        }

        void elipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetDragged(sender as DependencyObject, true);
            SetStartPoint(sender as DependencyObject, e.GetPosition(sender as IInputElement));
        }

        #region 線
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetDragged(canvas, true);
            SetStartPoint(canvas, e.GetPosition(canvas));
            Debug.WriteLine(e);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine(e);
            if (!GetDragged(canvas))
            {
                return;
            }

            Point prev = GetStartPoint(canvas);
            Point current = e.GetPosition(canvas);
            Line line = new Line();
            line.Stroke = Brushes.Black;
            line.X1 = prev.X;
            line.Y1 = prev.Y;
            line.X2 = current.X;
            line.Y2 = current.Y;
            canvas.Children.Add(line);

            SetStartPoint(canvas, current);
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetDragged(canvas, false);
            Debug.WriteLine(e);
        }
        #endregion

    }
}
