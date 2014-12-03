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

namespace AITradingProjectUI
{
    class EdgeDrawn
    {

        public CityDrawn city1 { get; private set; }
        public CityDrawn city2 { get; private set; }

        public int weight { get; private set; }


        public EdgeDrawn(CityDrawn c1, CityDrawn c2, int weight)
        {
            this.city1 = c1;
            this.city2 = c2;
            this.weight = weight;
        }


        public void Draw(Grid grid)
        {
           Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Blue;
            myLine.X1 = city1.x;
            myLine.X2 = city2.x;
            myLine.Y1 = city1.y;
            myLine.Y2 = city2.y;
            
            
            myLine.StrokeThickness = 1;
            grid.Children.Add(myLine);

            TextBlock weightText = new TextBlock{RenderTransform = new TranslateTransform((city1.x+city2.x)/2,(city1.y+city2.y)/2)};
            weightText.Text += weight;
            grid.Children.Add(weightText);
        }


        
    }
}
