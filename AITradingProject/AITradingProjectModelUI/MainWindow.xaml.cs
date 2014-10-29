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
using AITradingProjectModel.Model;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }


        public void draw(List<City> cities)
        {
            grid.Children.Clear();
            List<CityDrawn> list = new List<CityDrawn>();
            foreach(City c in cities)
            {
                CityDrawn b = new CityDrawn(c.ID, 3); //add base scale as size?
                list.Add(b);
            }            
            List<EdgeDrawn> traderoutes = new List<EdgeDrawn>();
            //basic setup. should be parameterized 
            grid.Height = 1000;
            grid.Width = 1000;
            double xMax = 500;
            double yMax = 500;
            this.Height = 1000;
            this.Width = 1000;

            double x =xMax;
            double y =yMax;

            //list size for stepsize. its halfed as we want to distribute cities in a circle.
            int size = list.Count/2;
            //step sizes
            double stepX=xMax/size;
            double stepY=yMax/size;
            //set x and y           
            foreach(CityDrawn cd in list)
            {
                
                cd.x = x;
                cd.y = y;
                if(x>xMax/2 && y>yMax/2)
                { //top right
                    x-=stepX;
                    y+=stepY;
                }
                else if(x>xMax/2 && y<yMax/2)
                {//lower right
                    x+=stepX;
                    y+=stepY;
                }
                else if(x<xMax/2 && y>yMax/2)
                {//top left
                    x-=stepX;
                    y-=stepY;
                }
                else
                {//lower left
                    x+=stepX;
                    y-=stepY;
                }
            }
            

            for (int i = 0; i < list.Count; i++)
            {
                list[i].draw(grid);
                for (int j = i; j < list.Count; j++)
                {
                    if (j == i) continue;
                    EdgeDrawn e = new EdgeDrawn(list[i], list[j], 4);
                    traderoutes.Add(e);
                    e.Draw(grid);
                }
            }                                
        }
    }
}
