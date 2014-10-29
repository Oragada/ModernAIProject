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
namespace UI
{
    class CityDrawn
    {
        public int ID {get; private set;}
        public double x {  get;  set; }
        public double y {  get;  set; }
        public int size { get; private set; }
        private int baseSize = 8;


        public CityDrawn(int id, int size)
        {
            ID = id;
            this.size = size;
        }

        public void draw(Grid grid)
        {
            double actualSize = baseSize* size;

            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(new Point(x + actualSize / 2, y + actualSize / 2));
            myPointCollection.Add(new Point(x + actualSize / 2, y - actualSize / 2));
            myPointCollection.Add(new Point(x - actualSize / 2, y - actualSize / 2));
            myPointCollection.Add(new Point(x - actualSize / 2, y + actualSize / 2));
            

            
            

            Polygon myPolygon = new Polygon();
            myPolygon.Points = myPointCollection;
            myPolygon.Fill = Brushes.Blue;            
            
            myPolygon.Stroke = Brushes.Black;
            myPolygon.StrokeThickness = 1;
            grid.Children.Add(myPolygon);


            /*
            // Create a red Ellipse.
            Ellipse myEllipse = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the  
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values.  
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            
            myEllipse.Width = baseSize*size;
            myEllipse.Height = baseSize * size;
            double left = x- (myEllipse.Width/2);
            double top = y-(myEllipse.Height/2);
            myEllipse.
            myEllipse.Margin = new Thickness(left, top, 0, 0);
            // Add the Ellipse to the StackPanel.
            grid.Children.Add(myEllipse);
            */

            

        }


    }
}
