using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    public class SemiSmartLudoPlayer : ILudoPlayer
    {
        LudoBoard board;
        Random rand;

        public SemiSmartLudoPlayer(LudoBoard board)
        {
            this.board = board;
            rand = new Random();
        }
        public void Play()
        {
            board.print("Semi Smart player playing");

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
            if (bestIndex != -1)
                board.moveBrick(bestIndex);
        }

        public float analyzeBrickSituation(int i)
        {
            if (board.moveable(i))
            {
                int[][] current_board = board.getBoardState();
                int[][] new_board = board.getNewBoardState(i, board.getMyColor(), board.getDice());

                if (hitOpponentHome(current_board, new_board))
                {
                    return 5 + rand.Next();
                }
                else if (hitMySelfHome(current_board, new_board))
                {
                    return (float)0.1;
                }
                else if (board.isStar(new_board[board.getMyColor()][i]))
                {
                    return 4 + rand.Next();
                }
                else if (moveOut(current_board, new_board))
                {
                    return 3 + rand.Next();
                }
                else if (board.atHome(new_board[board.getMyColor()][i], board.getMyColor()))
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


        private bool moveOut(int[][] current_board, int[][] new_board)
        {
            for (int i = 0; i < 4; i++)
            {
                if (board.inStartArea(current_board[board.getMyColor()][i], board.getMyColor()) && !board.inStartArea(new_board[board.getMyColor()][i], board.getMyColor()))
                {
                    return true;
                }
            }
            return false;
        }

        private bool hitOpponentHome(int[][] current_board, int[][] new_board)
        {
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
        private bool hitMySelfHome(int[][] current_board, int[][] new_board)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!board.inStartArea(current_board[board.getMyColor()][i], board.getMyColor()) && board.inStartArea(new_board[board.getMyColor()][i], board.getMyColor()))
                {
                    return true;
                }
            }
            return false;
        }

        public string PlayerName()
        {
            return "SemiSmartLudoPlayer";
        }
    }
}
