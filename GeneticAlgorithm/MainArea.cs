using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public partial class MainArea : Form
    {
        Population population = new Population();
        Graphics Sheet;
        Color[] ColorArea = new Color[6];
        object loker = new object();
        public MainArea()
        {
            InitializeComponent();
            Sheet = this.CreateGraphics();
            PreSeed();
            Task.Run(CheckNew);
        }
        public void PreSeed()
        {
            List<Individual> lst1 = new List<Individual>();
            List<Individual> lst2 = new List<Individual>();
            List<Individual> lst3 = new List<Individual>();
            List<Individual> lst4 = new List<Individual>();
            List<Individual> lst5 = new List<Individual>();
            List<Individual> lst6 = new List<Individual>();
            ColorArea[0] = Color.Red;
            ColorArea[1] = Color.SlateGray;
            ColorArea[2] = Color.Black;
            ColorArea[3] = Color.Green;
            ColorArea[4] = Color.Gold;
            ColorArea[5] = Color.Yellow;
            for (int i = 0; i < 2; i++)
            {
                Individual ind = new Individual(new Point(i * 10 + 100, i * 10 + 300), Color.Red);
                lst1.Add(ind);
            }
            population.Areas[0] = lst1;
            for (int i = 0; i < 2; i++)
            {
                Individual ind = new Individual(new Point(i * 20 + 100, i * 20 + 300), Color.SlateGray);
                lst2.Add(ind);
            }
            population.Areas[1] = lst2;

            for (int i = 0; i < 1; i++)
            {
                Individual ind = new Individual(new Point(i * 70 + 500, i * 35 + 200), Color.Black);
                lst3.Add(ind);
            }
            population.Areas[2] = lst3;
            for (int i = 0; i < 2; i++)
            {
                Individual ind = new Individual(new Point(i * 80 + 30, i * 60 + 300), Color.Green);
                lst4.Add(ind);
            }
            population.Areas[3] = lst4;
            for (int i = 0; i < 2; i++)
            {
                Individual ind = new Individual(new Point(i * 50 + 200, i * 40 + 160), Color.Gold);
                lst5.Add(ind);
            }
            population.Areas[4] = lst5;
            for (int i = 0; i < 2; i++)
            {
                Individual ind = new Individual(new Point(i * 60 + 300, i * 70), Color.Yellow);
                lst6.Add(ind);
            }
            population.Areas[5] = lst6;
            lst6.Clear();
        }

        private void PaintInd(object sender, PaintEventArgs e)
        {
            int q = 0;
            while (q < 1000)
            {
                e.Graphics.Clear(Color.White);
                foreach (IEnumerable<Individual> pop in population.Areas)
                {
                    foreach (Individual ind in pop)
                    {
                        ind.Update();
                        ind.LifeTime--;
                        e.Graphics.DrawRectangle(ind.pen, ind.rectangle);
                        ind.IsChecked = false;
                    }
                }
                Thread.Sleep(15);
                q++;
            }

        }
        public Color ColorOfRegion(Point p)
        {
            int reg = 0;
            if (p.X > 250)
            {
                if (p.Y > 300)
                {
                    if (p.Y < 600) reg = 4;
                    else reg = 5;
                }
                else { reg = 3; }
            }
            else
            {
                if (p.Y > 300)
                {
                    if (p.Y < 600) reg = 1;
                    else reg = 2;
                }
                else { reg = 0; }
            }

            return ColorArea[reg];
        }
        public void CheckNew()
        {
            lock (loker)
            {
                for (int i = 0; i < population.Areas.Count - 1; i++)
                {
                    Individual ind = population.Areas[i][0];
                    for (int j = 0; j < population.Areas[i].Count; j++)
                    {
                        if (population.Areas[i][j].LifeTime < 0)
                        {
                            Individual ind2 = population.Areas[i][j];
                            population.Areas[i].Remove(ind2);
                        }
                        else
                        {
                            if (!population.Areas[i][j].IsChecked)
                                if (Math.Sqrt(Math.Pow(ind.Center.X - population.Areas[i][j].Center.X, 2) + Math.Pow(ind.Center.Y - population.Areas[i][j].Center.Y, 2)) < ind.size)
                                {
                                    Individual some = ind.GenerateDescendant(ind, population.Areas[i][j], ColorOfRegion(ind.Center));
                                    population.Areas[i].Add(some);
                                    LogsWindow.Invoke((new MethodInvoker(delegate () { LogsWindow.AppendText("+ one descendant\n"); })));
                                    Sheet.DrawRectangle(some.pen, some.rectangle);
                                }
                        }

                    }
                    ind.IsChecked = true;
                }
            }
            Thread.Sleep(10);
        }
    }
}
