using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Circle
    {
        public int Rad;
        public int XC;
        public int YC;

        public void Drawcircle(Graphics g)
        {
            for (float i = 0; i < 360; i += 0.1f)
            {
                float thRadian = (float)(i * Math.PI / 180);
                float x = (float)(Rad * Math.Cos(thRadian));
                float y = (float)(Rad * Math.Sin(thRadian));
                x += XC;
                y += YC;
                g.FillEllipse(Brushes.Black, x - 7, y - 7, 7, 7);
            }
        }

        public PointF Movingball(float angle)
        {
            float thRadian = (float)(angle * Math.PI / 180);
            float x = (float)(Rad * Math.Cos(thRadian)) + XC;
            float y = (float)(Rad * Math.Sin(thRadian)) + YC;
            return new PointF(x, y);
        }
    }
}