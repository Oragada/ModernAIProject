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
using AITradingProject;
using System.Threading;
using System.Runtime.CompilerServices;


namespace AITradingProjectUI
{
    public class Worker
    {
        // This method will be called when the thread is started.
        public void DoWork()
        {
            int i = 0;
            int turns = 100;
            
            GameMaster master = new GameMaster(3);
            master.startGame();

            lock (MainWindow.cities)
            {
                MainWindow.cities = master.getCities();
            }
            while (i < 100)
            {
                lock (MainWindow.offers)
                {
                    i++;
                    Dictionary<Offer, TradeStatus> offers= master.RunTurn();
                    
                    MainWindow.offers = offers;

                    //MainWindow.reDraw();
                    i++;
                    
                    Thread.Sleep(100);
                }
                lock (MainWindow.syncLock)
                {
                    MainWindow.next = 0; ;
                }
                bool next = false;
                while (!next)
                {
                    lock (MainWindow.syncLock)
                    {
                        if (MainWindow.next != 0)
                        {
                            next = true;
                        }

                    }
                    Thread.Sleep(20);
                }

            }

        }
        public void RequestStop()
        {

        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<City> cities = new List<City>();
        private List<EdgeDrawn> traderoutes;
        public static Dictionary<Offer, TradeStatus> offers = new Dictionary<Offer,TradeStatus>();
        public static readonly object syncLock = new object();
        public static int next = 0;
        public MainWindow()
        {
            InitializeComponent();

            this.Height = 1000;
            this.Width = 1000;
            Worker workerObject = new Worker();
            Thread workerThread = new Thread(workerObject.DoWork);
            workerThread.Start();            
            MakeButton();
        }

        void MakeButton()
        {
            Button b2 = new Button();
            b2.Content = "Next Turn";
            // Associate event handler to the button. You can remove the event  
            // handler using "-=" syntax rather than "+=".
            b2.Click += new RoutedEventHandler(Onb2Click);
            b2.Height = 100;
            b2.Width = 100;
            
            grid.Children.Add(b2);
            //DockPanel.SetDock(b2, Dock.Top);


        }
        void Onb2Click(object sender, RoutedEventArgs e)
        {
            reDraw();
            lock (MainWindow.syncLock)
            {
                MainWindow.next = 1;
            }
            MakeButton();
        }

        public void reDraw()
        {
            lock (MainWindow.offers)
            {
                grid.Children.Clear();
                drawn.Clear();
                List<CityDrawn> list = new List<CityDrawn>();

                foreach (City c in cities)
                {
                    CityDrawn b = new CityDrawn(c.ID, 3); //add base scale as size?
                    list.Add(b);
                }
                traderoutes = new List<EdgeDrawn>();
                //basic setup. should be parameterized 
                grid.Height = 1000;
                grid.Width = 1000;
                double xMax = 300;
                double yMax = 300;


                double x = xMax;
                double y = yMax;

                //list size for stepsize. its halfed as we want to distribute cities in a circle.
                int size = list.Count / 2;
                //step sizes
                double stepX = xMax / size;
                double stepY = yMax / size;
                //set x and y           
                foreach (CityDrawn cd in list)
                {

                    cd.x = x;
                    cd.y = y;
                    if (x > xMax / 2 && y > yMax / 2)
                    { //top right
                        x -= stepX;
                        y += stepY;
                    }
                    else if (x > xMax / 2 && y < yMax / 2)
                    {//lower right
                        x += stepX;
                        y += stepY;
                    }
                    else if (x < xMax / 2 && y > yMax / 2)
                    {//top left
                        x -= stepX;
                        y -= stepY;
                    }
                    else
                    {//lower left
                        x += stepX;
                        y -= stepY;
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

                if (offers != null)
                {

                    foreach(Offer offer in offers.Keys)
                    {
                        TextBlock l = new TextBlock();

                        l.Text+="From: "+ offer.From.ID + " - ";
                        foreach(Resource res in offer.ResourcesOffered.Keys)
                        {
                            l.Text += res.ToString() + ": " + offer.ResourcesOffered[res]+ "  -";                            
                        }
                        EdgeDrawn e = search(offer.From.ID, offer.E.Other(offer.From).ID);
                        double theX = e.city1.ID==offer.From.ID ? e.city2.x : e.city1.x;
                        double theY = e.city1.ID == offer.From.ID ? e.city2.y+10 : e.city1.y+10;
                        while (drawn.ContainsKey(theX + "," + theY))
                        {
                            theY += 20;
                        }

                        l.RenderTransform = new TranslateTransform
                        {
                            X=theX,
                            Y=theY
                        };
                        grid.Children.Add(l);
                        drawn.Add(theX + "," + theY, true);
          
                        
                    }
                }
            }            
        }
        private Dictionary<String, bool> drawn = new Dictionary<string, bool>();

        private EdgeDrawn search(int from, int to)
        {
            foreach (EdgeDrawn e in traderoutes)
            {
                if (e.city1.ID == from && e.city2.ID == to || e.city2.ID == from && e.city1.ID == to)
                {
                    return e;
                }

            }
            return null;
        }
    }
}
