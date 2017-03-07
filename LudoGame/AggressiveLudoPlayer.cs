using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    public class AggressiveLudoPlayer : ILudoPlayer
    {
        LudoBoard board;
        Random rand;

        public AggressiveLudoPlayer(LudoBoard board)
        {
            this.board = board;
            this.rand = new Random();
        }

        public void Play()
        {
            board.print("Agressive player playing");

            int[] myBricksValue = new int[4];
            board.rollDice();
            float max = -1;
            int bestIndex = -1;
            for (int i = 0; i < 4; i++)
            {
                float value = analyzeBrickSituation(i);
                if (value > max && value > 0)
                {
                    bestIndex = i;
                    max = value;
                }
            }
            if (bestIndex != -1) board.moveBrick(bestIndex);
        }

        public float analyzeBrickSituation(int i)
        {
            if (board.moveable(i))
            {
                int[][] current_board = board.getBoardState();
                int[][] new_board = board.getNewBoardState(i, board.getMyColor(), board.getDice());

                if (hitOpponentHome(current_board, new_board))
                {
                    return 2 + rand.Next();
                }
                else
                {
                    return 1 + rand.Next();
                }
            }
            else
            {
                return 0;
            }
        }
        private bool isSafe(int index)
        {
            return board.isGlobe(index) || board.almostHome(index, board.getMyColor());
        }
        private bool hitOpponentHome(int[][] current_board, int[][] new_board)
        {
            int opponentsOnField = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board.getMyColor() != i)
                    {
                        if (board.atField(current_board[i][j]) && !board.atField(new_board[i][j]))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public string PlayerName()
        {
            return "AggressiveLudoPlayer";
        }
    }
}
