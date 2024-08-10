using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Curve
    {
        public List<PointF> ControlPoints = new List<PointF>();

        public float t_inc = 0.001f;

        public Curve()
        {
            ControlPoints = new List<PointF>();
        }

        private float Factorial(int n)
        {
            float res = 1.0f;

            for (int i = 2; i <= n; i++)
                res *= i;

            return res;
        }

        private float C(int n, int i)
        {
            float res = Factorial(n) / (Factorial(i) * Factorial(n - i));
            return res;
        }

        private double Calc_B(float t, int i)
        {
            int n = ControlPoints.Count - 1;
            double res = C(n, i) *
                            Math.Pow((1 - t), (n - i)) *
                            Math.Pow(t, i);
            return res;
        }

        public PointF CalcCurvePointAtTime(float t)
        {
            PointF pt = new PointF();
            for (int i = 0; i < ControlPoints.Count; i++)
            {
                float B = (float)Calc_B(t, i);
                pt.X += B * ControlPoints[i].X;
                pt.Y += B * ControlPoints[i].Y;
            }
            return pt;
        }

        public void SetControlPoint(PointF pt)
        {
            ControlPoints.Add(pt);
        }

        public void DrawCurve(Graphics g)
        {
            if (ControlPoints.Count < 3)
                return;

            for (float t = 0; t <= 1; t += 0.001f)
            {
                PointF pt = CalcCurvePointAtTime(t);
                g.FillEllipse(Brushes.Black, pt.X - 2, pt.Y - 2, 4, 4);
            }
        }
    }
}
