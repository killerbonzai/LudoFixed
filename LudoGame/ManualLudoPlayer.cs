using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    class ManualLudoPlayer : ILudoPlayer
    {
        LudoBoard board;

        public ManualLudoPlayer(LudoBoard board)
        {
            this.board = board;
        }

        public string PlayerName()
        {
            return "MyName";
        }
        
        public void Play()
        {
            // do stuff
            // do UI first
        }
    }
}
