using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Robot_Control
{
    public partial class FormCalibration : Form
    {
        public bool manual = false;
        public bool auto = false;
        public bool update = false;

        Point[] ps = new Point[15];
        Point[] rps = new Point[15];
        public int[] values = new int[15];
        int radius = 3;
        bool moving = false;
        int selectedPoint;

        public FormCalibration(int[] v)
        {
            InitializeComponent();
            for (int i = 0; i < 15; i++)
                values[i] = v[i];

            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Point(graphX(i), graphY(i));
                rps[i] = new Point(graphX(i), graphY(reverse(i)));
            }
        }

        int reverse(int i)
        {
            return ps.Length - 1 - i;
        }


        private int graphX(int x)
        {
            int X = x * pictureBox1.Width / (ps.Length - 1);
            if (x == ps.Length - 1)
                X -= 3;
            return X;
        }

        private int graphY(int i)
        {
            int s = pictureBox1.Height / 2;
            return -values[i] * s / 255 + s;
        }

        private void endPoint(Point p, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(
                p.X - radius, p.Y - radius,
                2 * radius + 1, 2 * radius + 1);
            e.Graphics.FillEllipse(Brushes.White, rect);
            e.Graphics.DrawEllipse(Pens.Black, rect);
        }

        private void drawGrid(int h, int v, PaintEventArgs e)
        {
            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, pictureBox1.Height);
            Pen bold = new Pen(Color.Black, 2);
            int inc = pictureBox1.Width / h;
            for (int i = 0; i < h; i++)
            {
                p1.X = p2.X = i * pictureBox1.Width / (ps.Length - 1);
                if (i == (h - 1) / 2)
                    e.Graphics.DrawLine(bold, p1, p2);
                else
                    e.Graphics.DrawLine(Pens.Gray, p1, p2);
            }
            inc = pictureBox1.Height / v;
            p1.X = 0;
            p2.X = pictureBox1.Width;
            for (int i = 0; i < v; i++)
            {
                p1.Y = p2.Y = i * pictureBox1.Height / v;
                if (i == v / 2)
                    e.Graphics.DrawLine(bold, p1, p2);
                else
                    e.Graphics.DrawLine(Pens.Gray, p1, p2);
            }
        }

        private int FindDistanceToPointSquared(Point P1, Point P2)
        {
            int x = P1.X - P2.X;
            int y = P1.Y - P2.Y;
            return (x * x + y * y);
        }

        private bool MouseIsOverEndpoint(Point mouse_Point, out int hit_Point)
        {
            for (int i = 0; i < ps.Length; i++)
            {
                if (FindDistanceToPointSquared(mouse_Point, ps[i]) < radius * radius)
                {
                    hit_Point = i;
                    return true;
                }
            }
            hit_Point = -1;
            return false;
        }

        private int constrain(int i, int min, int max)
        {
            if (i < min)
                i = min;
            else if (i > max)
                i = max;
            return i;
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            manual = true;
            this.Close();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            auto = true;
            this.Close();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            moving = moving && e.Button == MouseButtons.Left;
            if (moving)
            {
                ps[selectedPoint].Y = e.Location.Y;
                ps[selectedPoint].Y = constrain(ps[selectedPoint].Y, 0, pictureBox1.Height);
                rps[reverse(selectedPoint)].Y = ps[selectedPoint].Y;
                values[selectedPoint] = constrain((pictureBox1.Height / 2 - ps[selectedPoint].Y) * 512 / (pictureBox1.Height), -255, 255);
                pictureBox1.Invalidate();

            }
            else
            {
                Cursor new_cursor = Cursors.Cross;

                if (MouseIsOverEndpoint(e.Location, out selectedPoint))
                    new_cursor = Cursors.Hand;

                // Set the new cursor.
                if (pictureBox1.Cursor != new_cursor)
                    pictureBox1.Cursor = new_cursor;

                moving = selectedPoint != -1 && e.Button == MouseButtons.Left;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            drawGrid(ps.Length + 1, 16, e);
            for (int i = 0; i < ps.Length - 1; i++)
            {
                e.Graphics.DrawLine(Pens.Blue, ps[i], ps[i + 1]);
                e.Graphics.DrawLine(Pens.Red, rps[i], rps[i + 1]);
            }
            foreach (Point p in ps)
                endPoint(p, e);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            update = true;
            this.Close();
        }
    }
}
