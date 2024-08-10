using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    class Rollercoaster
    {
        public DDA line = null;
        public Circle circle = null;
        public Curve curve = null;
        public float EndX = 200, EndY = 700;
    }

    public class Player
    {
        public float X = 0, Y = 700;
        public float time = 0.01f; // For Curve
        public float angle = 90; // For Circle
        public float speed = 0;
    }

    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        Bitmap off;
        List<Rollercoaster> rollercoasters = new List<Rollercoaster>();
        Player player = new Player();
        bool start = false;
        int index = 0;

        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            Text = "Rollercoaster!!";
            Load += Form1_Load;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {DrawDubb(e.Graphics);}

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (start && index < rollercoasters.Count)
            {
                if (player.speed < 0) player.speed = 0;
                if (rollercoasters[index].line != null)
                {
                    if (rollercoasters[index].line.CalcNextPoint(player))
                    {
                        player.X += player.speed;
                    }
                    else
                    {
                        player.X = rollercoasters[index].EndX;
                        player.Y = rollercoasters[index].EndY;
                        index++;
                    }
                }
                else if (rollercoasters[index].circle != null)
                {
                    player.angle -= 5 + player.speed;
                    PointF point = rollercoasters[index].circle.Movingball(player.angle);
                    player.X = point.X;
                    player.Y = point.Y;
                    if (player.angle <= -270)
                    {
                        player.angle = 90;
                        player.X = rollercoasters[index].EndX;
                        player.Y = rollercoasters[index].EndY;
                        index++;
                    }
                }
                else if (rollercoasters[index].curve != null)
                {
                    if (player.time <= 1.0f)
                    {
                        PointF point = rollercoasters[index].curve.CalcCurvePointAtTime(player.time);
                        player.X = point.X;
                        player.Y = point.Y;
                        player.time += 0.01f + (player.speed / 100);
                    }
                    else
                    {
                        player.X = rollercoasters[index].EndX;
                        player.Y = rollercoasters[index].EndY;
                        player.time = 0.01f;
                        index++;
                    }
                }

            }
            DrawDubb(CreateGraphics());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            float Xs = 0, Ys = 700, Xe = 200, Ye = 700;
            int XC = 0, YC = 350, rad = 150;
            switch (e.KeyCode)
            {
                // For DDA line
                case Keys.D1:
                    if (rollercoasters.Count > 0)
                    {
                        int last_creation = rollercoasters.Count - 1;
                        Xs = rollercoasters[last_creation].EndX;
                        Ys = rollercoasters[last_creation].EndY;
                        Xe = Xs + 200;
                        Ye = Ys;
                    }
                    DDA line = new DDA();
                    line.Xs = Xs;
                    line.Ys = Ys;
                    line.Xe = Xe;
                    line.Ye = Ye;
                    Rollercoaster line_pnn = new Rollercoaster();
                    line_pnn.line = line;
                    line_pnn.EndX = Xe;
                    line_pnn.EndY = Ye;
                    rollercoasters.Add(line_pnn);
                    break;
                // For Circle
                case Keys.D2:
                    if (rollercoasters.Count == 0 || rollercoasters[rollercoasters.Count - 1].curve != null)
                    {
                        MessageBox.Show("You need to add a line before adding the circle");
                        break;
                    }
                    else if (rollercoasters[rollercoasters.Count - 1].circle != null)
                    {
                        MessageBox.Show("You can't add 2 circles in a row");
                        break;
                    }
                    if (rollercoasters.Count > 0)
                    {
                        int last_creation = rollercoasters.Count - 1;
                        XC = (int)rollercoasters[last_creation].EndX;
                        YC = (int)rollercoasters[last_creation].EndY - rad;
                    }
                    Circle circle = new Circle();
                    circle.XC = XC;
                    circle.YC = YC;
                    circle.Rad = rad;
                    Rollercoaster circle_pnn = new Rollercoaster();
                    circle_pnn.circle = circle;
                    circle_pnn.EndX = XC;
                    circle_pnn.EndY = YC + rad;
                    rollercoasters.Add(circle_pnn);
                    break;
                // For Curve
                case Keys.D3:
                    if (rollercoasters.Count > 0)
                    {
                        int last_creation = rollercoasters.Count - 1;
                        Xs = rollercoasters[last_creation].EndX;
                        Ys = rollercoasters[last_creation].EndY;
                        Xe = Xs + 300;
                        Ye = Ys;
                    }
                    Curve curve = new Curve();
                    curve.SetControlPoint(new PointF(Xs, Ys));
                    curve.SetControlPoint(new PointF(Xs + 150, Ys - 400));
                    curve.SetControlPoint(new PointF(Xe, Ye));
                    Rollercoaster curveRollercoaster = new Rollercoaster();
                    curveRollercoaster.curve = curve;
                    curveRollercoaster.EndX = Xe;
                    curveRollercoaster.EndY = Ye;
                    rollercoasters.Add(curveRollercoaster);
                    break;
                // To delete last part
                case Keys.D4:
                    if (rollercoasters.Count > 0)
                    {
                        rollercoasters.RemoveAt(rollercoasters.Count - 1);
                    }
                    break;
                // To increase the stats of the current algorithm
                case Keys.Up:
                    if (rollercoasters.Count > 0)
                    {
                        int last_creation = rollercoasters.Count - 1;
                        if (rollercoasters[last_creation].line != null)
                        {
                            rollercoasters[last_creation].line.Xe += 5;
                            rollercoasters[last_creation].EndX += 5;
                        }
                        else if (rollercoasters[last_creation].circle != null)
                        {
                            rollercoasters[last_creation].circle.YC -= 5;
                            rollercoasters[last_creation].circle.Rad += 5;
                        }
                        else if (rollercoasters[last_creation].curve != null)
                        {
                            rollercoasters[last_creation].curve.ControlPoints[1] = new PointF(rollercoasters[last_creation].curve.ControlPoints[1].X, rollercoasters[last_creation].curve.ControlPoints[1].Y - 10);
                        }
                    }
                    break;
                // To decrease the stats of the current algorithm
                case Keys.Down:
                    if (rollercoasters.Count > 0)
                    {
                        int last_creation = rollercoasters.Count - 1;
                        if (rollercoasters[last_creation].line != null)
                        {
                            rollercoasters[last_creation].line.Xe -= 5;
                            rollercoasters[last_creation].EndX -= 5;
                        }
                        else if (rollercoasters[last_creation].circle != null)
                        {
                            rollercoasters[last_creation].circle.YC += 5;
                            rollercoasters[last_creation].circle.Rad -= 5;
                        }
                        else if (rollercoasters[last_creation].curve != null)
                        {
                            rollercoasters[last_creation].curve.ControlPoints[1] = new PointF(rollercoasters[last_creation].curve.ControlPoints[1].X, rollercoasters[last_creation].curve.ControlPoints[1].Y + 10);
                        }
                    }
                    break;
                // To decrease speed of simulation
                case Keys.Left:
                    player.speed -= 1;
                    break;
                // To increase speed of simulation
                case Keys.Right:
                    player.speed += 1;
                    break;
                // Start game
                case Keys.Space:
                    if (rollercoasters.Count > 0)
                    {
                        if (start)
                        {
                            start = false;
                            player = new Player();
                        }
                        else start = true;
                    }
                    else MessageBox.Show("There is nothing to ride on");
                    break;
            }
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            for (int i = 0; i < rollercoasters.Count;i++)
            {
                if (rollercoasters[i].line != null)
                {
                    g.DrawLine(new Pen(Color.Black, 5), rollercoasters[i].line.Xs, rollercoasters[i].line.Ys, rollercoasters[i].line.Xe, rollercoasters[i].line.Ye);
                }
                else if (rollercoasters[i].circle != null)
                {
                    rollercoasters[i].circle.Drawcircle(g);
                }
                else if (rollercoasters[i].curve != null)
                {
                    rollercoasters[i].curve.DrawCurve(g);
                }
            }
            if (start) g.FillEllipse(new SolidBrush(Color.Orange), player.X - 10, player.Y - 10, 20, 20);
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}