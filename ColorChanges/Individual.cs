using System;
using System.Drawing;


namespace ColorChanges
{ 
    public static class Direction
    {
        public const int left = 1;
        public const int right = 2;
        public const int up = 3;
        public const int down = 4;
    }
    public class Individual
    {
        public int LifeTime { get; set; }
        public bool IsChecked { get; set; }
        public int Step = 10;
        public int size = 40;
        public Point Center;

        public Color ColorOfInd;
        public Int32 bitesColor { get; set; } // а нам нужен вектор, массив 
        public bool[] Chromosome = new bool[32];

        public int ChangeDirection;
        public int DirectionNow;

        public Rectangle rectangle;
        public Pen pen;
        public Size s;

        public Individual(Point center, Color color)
        {
            ColorOfInd = color;
            LifeTime = 200;
            IsChecked = false;
            bitesColor = 0;
            bool[] temp = ChromosomConvert.FromColorToBool(color);
            for (int i = 0; i < 32; i++)
                Chromosome[i] = temp[i];
            Center = center;
            s.Height = size;
            s.Width = size;
            rectangle = new Rectangle(Center, s);
            pen = new Pen(color, size);
            Random rnd = new Random();
            ChangeDirection = rnd.Next(3,25);
        }
        public void SetCenter(Point p)
        {
            Center = p;
            rectangle = new Rectangle(Center, s);
        }
        
        // 
        public string ChromosomeToString()
        {
            string res = String.Empty;
            for(int i =0;i<32;i++)
            {
                if (Chromosome[i])
                    res += "1";
                else res += "0";
            }
            return res;
        }
        
       


        public Individual GenerateDescendant(Individual Parent1, Individual Parent2, Color TargetColor)
        {
            Parent1.IsChecked = true;
            Parent2.IsChecked = true;

            Individual Chield = new Individual(new Point(), Color.White);
            // Кросинговер определяем точку разрыва
            int BreakPoint = RandomNumber.GetValue(2, 30);
            bool[] chield1 = new bool[32];
            bool[] chield2 = new bool[32];
            for (int i = 0; i < BreakPoint; i++) 
            {
                chield1[i] = Parent1.Chromosome[i];
                chield2[i] = Parent2.Chromosome[i];
            }
            for (int j = BreakPoint; j < 32; j++) 
            {
                chield1[j] = Parent2.Chromosome[j];
                chield2[j] = Parent1.Chromosome[j];
            }
            // Мутация - замена случайного бита 
            // Задаем вероятность мутации для каждого из потомков
            double MutationProb = RandomNumber.GetValue(1, 10) / 10;// Вероятность мутации 
            Random randDouble = new Random(RandomNumber.GetValue(1,100));
            double MutationProb1 = randDouble.NextDouble();
            double MutationProb2 = randDouble.NextDouble();
            // Задаем случайный номер бита для замены
            // не учитываем первый и последний
            int Bit1 = RandomNumber.GetValue(0, 31);
            int Bit2 = RandomNumber.GetValue(0, 31);
            if(MutationProb1 < MutationProb)
            {
                chield1[Bit1] = !chield1[Bit1];
            }
            if(MutationProb2 < MutationProb)
            {
                chield2[Bit2] = !chield2[Bit2];
            }
            //// проверка на живучость
            Int32 num1 = Convert.ToInt32(ChromosomConvert.FromBoolToColor(chield1).ToArgb());
            Int32 num2 = Convert.ToInt32(ChromosomConvert.FromBoolToColor(chield2).ToArgb());
            // Целевая функция близость кода цвета к коду цвета региона в котором находятся родители
            Int32 RegionColor = TargetColor.ToArgb();
            if (Math.Abs(num1 - RegionColor) < Math.Abs(num2 - RegionColor))
            {
                for (int i = 0; i < 32; i++)
                    Chield.Chromosome[i] = chield1[i];
                Chield.bitesColor = num1;
                Chield.Center = Parent1.Center;
                Chield.Center.X += 30;
                Chield.Center.Y += 10;
                Color col = ChromosomConvert.FromBoolToColor(chield1);
                Chield.pen.Color = col;
                Chield.ColorOfInd = col;
            }
            else
            {
                for (int i = 0; i < 32; i++)
                    Chield.Chromosome[i] = chield2[i];
                Chield.bitesColor = num2;
                Chield.Center = Parent2.Center;
                Chield.Center.X += 30;
                Chield.Center.Y += 10;
                Color col = ChromosomConvert.FromBoolToColor(chield2);
                Chield.pen.Color = col;
                Chield.ColorOfInd = col;
            }
            return Chield;
        }

        public void Move()
        {
            int rnd;
            if(LifeTime != 0)
            {
                if (ChangeDirection <= 0)
                { // перемещение центра на указаный шаг в случайном направлении
                    rnd = RandomNumber.GetValue(0, 5);
                    ChangeDirection = RandomNumber.GetValue(0, 28);
                }
                else { ChangeDirection--; rnd = DirectionNow; }
                
                    switch (rnd)
                    {
                        case 1: { this.Center.X -= Step; DirectionNow = rnd; break; }
                        case 2: { this.Center.X += Step; DirectionNow = rnd; break; }
                        case 3: { this.Center.Y += Step; DirectionNow = rnd; break; }
                        case 4: { this.Center.Y -= Step; DirectionNow = rnd; break; }
                    }
                if (Center.X > 900)
                    DirectionNow = 1;
                if (Center.X < 0)
                    DirectionNow = 2;
                if (Center.Y > 500)
                    DirectionNow = 4;
                if (Center.Y < 0)
                    DirectionNow = 3;
                rectangle = new Rectangle(Center, s);
            }
        }
    }
}
