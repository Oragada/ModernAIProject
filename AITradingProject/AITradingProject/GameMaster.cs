using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITradingProject.Model;
using AITradingProject.Agent;

namespace AITradingProject
{
    public class GameMaster
    {
        private Dictionary<int, Agent.Agent> agents;
        private GameState game;

        public GameMaster(int cityNum)
        {
            game = new GameState(cityNum);

            agents = new Dictionary<int, Agent.Agent>();

            for (int i = 0; i < cityNum; i++)
            {
                agents.Add(i, new RandomAgent());
            }
        }

        private void RunTurn()
        {
            //TODO
            //Production

            //Trade

            //Consumption
        }

        public void startGame()
        {
            //TODO
            //go through turns until something is achieved.
            //function is called from program.
        }
    }

    
}
