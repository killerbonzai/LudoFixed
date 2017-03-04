using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LudoGame
{
    public class Ludo
    {
        private static LudoBoard board;
        public static bool visual = true;

        public Ludo()
        {
            board = new LudoBoard();
            //setVisible(visual);
            // this is where Adam does some UI stuff. Like add a menu and add the board to the center of the window

            play();
        }

        // this was used for the Reset Game menu button in the UI
        //public void actionPerformed(ActionEvent event)
        //{
        //    if (event.getActionCommand()=="Reset Game")
        //    {
        //        board.kill();
        //    }
        //}

        public void play()
        {
            //System.out.println("Playing Ludo");
            //long time = System.currentTimeMillis();
            int[] result = new int[4];
            //board.setPlayer(new ManualLUDOPlayer(board), LudoBoard.YELLOW);
            board.setPlayer(new RandomLudoPlayer(board),LudoBoard.YELLOW);
            //board.setPlayer(new MinLUDOPlayer(board), LudoBoard.YELLOW);
            board.setPlayer(new RandomLudoPlayer(board), LudoBoard.RED);
            board.setPlayer(new RandomLudoPlayer(board), LudoBoard.BLUE);
            board.setPlayer(new RandomLudoPlayer(board), LudoBoard.GREEN);
            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    board.play();
                    board.kill();

                    result[0] += board.getPoints()[0];
                    result[1] += board.getPoints()[1];
                    result[2] += board.getPoints()[2];
                    result[3] += board.getPoints()[3];

                    board.reset();
                    board.setPlayer(new RandomLudoPlayer(board), LudoBoard.YELLOW);
                    board.setPlayer(new RandomLudoPlayer(board), LudoBoard.RED);
                    board.setPlayer(new RandomLudoPlayer(board), LudoBoard.BLUE);
                    board.setPlayer(new RandomLudoPlayer(board), LudoBoard.GREEN);
                    //if ((i % 500) == 0) System.out.print(".");
                }
            }
            catch (Exception)
            {
                throw;
            }
            //System.out.println();
            //System.out.println("Simulation took " + (System.currentTimeMillis() - time) + " miliseconds");
            //System.out.println("RESULT:");
            //System.out.println("YELLOW Player: " + result[0]);
            //System.out.println("RED    Player: " + result[1]);
            //System.out.println("BLUE   Player: " + result[2]);
            //System.out.println("GREEN  Player: " + result[3]);

        }
    }
}
