using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LudoGame;
using System.Diagnostics;

namespace NoGUI
{
    class Program
    {
        LudoBoard board;
        

        static void Main(string[] args)
        {
            new Program().Run();
        }

        public void Run()
        {
            board = new LudoBoard();
            
            Play();
        }

        private void AutoPlay() // a lot like Adam did it
        {
            Stopwatch swatch = new Stopwatch();
            swatch.Start();
            int[] result = new int[4];
            board.setPlayer(new RandomLudoPlayer(board), LudoBoard.YELLOW);
            board.setPlayer(new FifoLudoPlayer(board), LudoBoard.RED);
            board.setPlayer(new RandomLudoPlayer(board), LudoBoard.BLUE);
            board.setPlayer(new FifoLudoPlayer(board), LudoBoard.GREEN);
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
                    board.setPlayer(new FifoLudoPlayer(board), LudoBoard.RED);
                    board.setPlayer(new RandomLudoPlayer(board), LudoBoard.BLUE);
                    board.setPlayer(new FifoLudoPlayer(board), LudoBoard.GREEN);
                    if ((i % 500) == 0) Console.Write(".");
                }
            }
            catch (Exception)
            {
                throw;
            }

            swatch.Stop();
            Console.WriteLine();
            Console.WriteLine("Simulation took " + swatch.ElapsedMilliseconds + " miliseconds");
            Console.WriteLine("RESULT:");
            Console.WriteLine("YELLOW Player: " + result[0]);
            Console.WriteLine("RED    Player: " + result[1]);
            Console.WriteLine("BLUE   Player: " + result[2]);
            Console.WriteLine("GREEN  Player: " + result[3]);

            Console.ReadKey();
        }

        private void Play() // some minor changes
        {
            Stopwatch swatch = new Stopwatch();
            swatch.Start();

            List<ILudoPlayer> playerList = board.GetPlayerList;
            ILudoPlayer[] activePlayers = new ILudoPlayer[4];
            int[] result = new int[4];

            // show list in ui
            // pick what type should be play each

            activePlayers[0] = playerList[0];
            activePlayers[1] = playerList[1];
            activePlayers[2] = playerList[2];
            activePlayers[3] = playerList[3];
            board.setAllPlayers(activePlayers[0], activePlayers[1], activePlayers[2], activePlayers[3]);

            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    board.play();
                    board.kill();

                    int[] points = board.getPoints();
                    result[0] += points[0];
                    result[1] += points[1];
                    result[2] += points[2];
                    result[3] += points[3];

                    board.reset();
                    board.setAllPlayers(activePlayers[0], activePlayers[1], activePlayers[2], activePlayers[3]);
                    //board.setPlayer(activePlayers[0], LudoBoard.YELLOW);
                    //board.setPlayer(activePlayers[1], LudoBoard.RED);
                    //board.setPlayer(activePlayers[2], LudoBoard.BLUE);
                    //board.setPlayer(activePlayers[3], LudoBoard.GREEN);
                    if ((i % 500) == 0) Console.Write(".");
                }
            }
            catch (Exception)
            {
                throw;
            }

            swatch.Stop();
            Console.WriteLine();
            Console.WriteLine("Simulation took " + swatch.ElapsedMilliseconds + " miliseconds");
            Console.WriteLine("RESULT:");
            Console.WriteLine("YELLOW Player: " + result[0] + " - (" + activePlayers[0].PlayerName() + ")");
            Console.WriteLine("RED    Player: " + result[1] + " - (" + activePlayers[1].PlayerName() + ")");
            Console.WriteLine("BLUE   Player: " + result[2] + " - (" + activePlayers[2].PlayerName() + ")");
            Console.WriteLine("GREEN  Player: " + result[3] + " - (" + activePlayers[3].PlayerName() + ")");

            Console.ReadKey();
        }
    }
}