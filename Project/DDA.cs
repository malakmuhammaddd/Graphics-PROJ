using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class DDA
    {
        public float Xs, Ys, Xe, Ye, cx;
        public bool CalcNextPoint(Player player)
        {
            if (Xs < Xe)
            {
                player.X += 5;
                if (player.X >= Xe)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
