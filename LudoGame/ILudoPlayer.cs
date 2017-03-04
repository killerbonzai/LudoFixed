using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    public interface ILudoPlayer : LudoAiSDK.IAiPlugin
    {
        new void Play();
    }
}
