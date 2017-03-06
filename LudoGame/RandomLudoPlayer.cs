using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    public class RandomLudoPlayer : ILudoPlayer
    {
        LudoBoard board;
        Random rand;

        public RandomLudoPlayer(LudoBoard board)
        {
            this.board = board;
            rand = new Random();
        }

        public void Play()
        {
            board.print("Random player playing");
            board.rollDice();
            int nr = -1;
            double best = 0;
            for (int i = 0; i < 4; i++) // find a random moveable brick
            {
                if (board.moveable(i))
                {
                    double temp = rand.NextDouble();
                    if (temp > best)
                    {
                        best = temp;
                        nr = i;
                    }
                }
            }
            if (nr != -1) board.moveBrick(nr);
            //else nothing to do - no moveable bricks
        }

        public void delay()
        {
            try
            {
                //wait(5000);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
