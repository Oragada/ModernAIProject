using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITradingProject
{
    class Program
    {
        static void Main(string[] args)
        {

            GameMaster master = new GameMaster(3);
            master.startGame();
        }
    }
}
