using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProject.NEATExperiment;

namespace AITradingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            NEATProgram.Run();
            GameMaster master = new GameMaster(3);
            master.startGame();
        }
    }
}
