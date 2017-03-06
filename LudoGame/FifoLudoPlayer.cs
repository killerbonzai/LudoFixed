using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    class FifoLudoPlayer : ILudoPlayer
    {
        LudoBoard board;

        public FifoLudoPlayer(LudoBoard bord)
        {
            this.board = bord;
        }

        public void Play()
        {
            board.print("Fifo player playing");
            board.rollDice();
            for (int i = 0; i < 4; i++)
            {
                if (board.moveable(i))
                {
                    board.moveBrick(i);
                    return;
                }
            }
        }
    }
}
