using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ColorChanges
{
    public partial class TestIndivid : Form
    {
        Graphics g;
        public TestIndivid()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
        }
        public void Start()
        {

            List<Individual> individs = new List<Individual>();
            Color[] color = new Color[4];
            color[0] = Color.Yellow;
            color[1] = Color.Turquoise;
            color[2] = Color.Black;
            color[3] = Color.White;

            richTextBox1.AppendText(Color.Red.ToArgb().ToString());
            // рисуем все цвета
            Rectangle rec = new Rectangle(10, 10, 10, 50);
            Pen pen = new Pen(Color.FromArgb(-65536), 10);
            for (int i = -65526; i < -65280; i+=10)
            {
                g.DrawRectangle(pen, rec);
                pen.Color = Color.FromArgb(i);
                rec.Location = new Point(20 + rec.Location.X,rec.Location.Y);
            }
            richTextBox1.AppendText(Color.Orange.ToArgb().ToString());
            Rectangle rec2 = new Rectangle(10, 70, 10, 50);
            Pen pen2 = new Pen(Color.FromArgb(-25600), 10);
            for (int i = -25590; i < -25340; i += 10)//590-340=250
            {
                g.DrawRectangle(pen2, rec2);
                pen2.Color = Color.FromArgb(i);
                rec2.Location = new Point(20 + rec2.Location.X, rec2.Location.Y);
            }
            richTextBox1.AppendText(Color.Green.ToArgb().ToString());
            Rectangle rec3 = new Rectangle(10, 130, 10, 50);
            Pen pen3 = new Pen(Color.FromArgb(-16744448), 10);
            for (int i = -16744448; i < -16744200; i += 10)//590-340=250
            {
                g.DrawRectangle(pen3, rec3);
                pen3.Color = Color.FromArgb(i);
                rec3.Location = new Point(20 + rec3.Location.X, rec3.Location.Y);
            }
            richTextBox1.AppendText(Color.Gainsboro.ToArgb().ToString());
            Rectangle rec4 = new Rectangle(10, 190, 10, 50);
            Pen pen4 = new Pen(Color.FromArgb(-2302706), 10);
            for (int i = -2302706; i < -2302460; i += 10)//590-340=250
            {
                g.DrawRectangle(pen4, rec4);
                pen4.Color = Color.FromArgb(i);
                rec4.Location = new Point(20 + rec4.Location.X, rec4.Location.Y);
            }

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Draw_Click(object sender, EventArgs e)
        {
            Start();
        }
    }
}
