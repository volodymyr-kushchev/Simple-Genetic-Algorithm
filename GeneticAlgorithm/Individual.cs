using System;
using System.Drawing;


namespace GeneticAlgorithm
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
            bool[] temp = FromColorToBool(color);
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
        // Gets bool array represents 32 bits color and convert into struct color
        // Returns Color
        private Color FromBoolToColor(bool[] arr)
        {
            bool[] res = new bool[32];
            bool[] b0 = new bool[8], b1 = new bool[8], b2 = new bool[8], b3 = new bool[8];
            for (int i = 0; i < 8; i++)
                b0[i] = arr[i];
            for (int i = 8; i < 16; i++)
                b1[(i - 8)] = arr[i];
            for (int i = 16; i < 24; i++)
                b2[(i - 16)] = arr[i];
            for (int i = 24; i < 32; i++)
                b3[(i - 24)] = arr[i];
            Int32 n0 = Convert.ToInt32(ConvertBoolArrayToByte(b0));
            Int32 n1 = Convert.ToInt32(ConvertBoolArrayToByte(b1));
            Int32 n2 = Convert.ToInt32(ConvertBoolArrayToByte(b2));
            Int32 n3 = Convert.ToInt32(ConvertBoolArrayToByte(b3));
            Color color = Color.FromArgb(n0, n1, n2, n3);
            return color;

        }
        // Gets color and convert to bool array 
        // Returns bool array
        public bool[] FromColorToBool(Color color)
        {
            bool[] res = new bool[32];
            bool[] b0 = ConvertByteToBoolArray(color.A);
            for (int i = 0; i < 8; i++)
                res[i] = b0[i];
            bool[] b1 = ConvertByteToBoolArray(color.R);
            for (int i = 8; i < 16; i++)
                res[i] = b1[(i - 8)];
            bool[] b2 = ConvertByteToBoolArray(color.G);
            for (int i = 16; i < 24; i++)
                res[i] = b2[(i - 16)];
            bool[] b3 = ConvertByteToBoolArray(color.B);
            for (int i = 24; i < 32; i++)
                res[i] = b3[(i - 24)];
            return res;

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
        private static byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }
        // Auxiliary method
        private bool[] ConvertByteToBoolArray(byte b)
        {
            // prepare the return result
            bool[] result = new bool[8];

            // check each bit in the byte. if 1 set to true, if 0 set to false
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            // reverse the array
            //Array.Reverse(result);

            return result;
        }


        public Individual GenerateDescendant(Individual Parent1, Individual Parent2, Color TargetColor)
        {
            Parent1.IsChecked = true;
            Parent2.IsChecked = true;

            Individual Chield = new Individual(new Point(), Color.White);
            // Кросинговер определяем точку разрыва
            Random rand = new Random();
            int BreakPoint = rand.Next(2, 30);
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
            double MutationProb = rand.Next(1, 10) / 10;// Вероятность мутации 
            Random randDouble = new Random(rand.Next(1,100));
            double MutationProb1 = randDouble.NextDouble();
            double MutationProb2 = randDouble.NextDouble();
            // Задаем случайный номер бита для замены
            // не учитываем первый и последний
            int Bit1 = rand.Next(0, 31);
            int Bit2 = rand.Next(0, 31);
            if(MutationProb1 < MutationProb)
            {
                chield1[Bit1] = !chield1[Bit1];
            }
            if(MutationProb2 < MutationProb)
            {
                chield2[Bit2] = !chield2[Bit2];
            }
            //// проверка на живучость
            Int32 num1 = Convert.ToInt32(FromBoolToColor(chield1).ToArgb());
            Int32 num2 = Convert.ToInt32(FromBoolToColor(chield2).ToArgb());
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
                Color col = FromBoolToColor(chield1);
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
                Color col = FromBoolToColor(chield2);
                Chield.pen.Color = col;
                Chield.ColorOfInd = col;
            }
            return Chield;
        }

        public void Update()
        {
            int rnd;
            Random rand = new Random();
            if(LifeTime != 0)
            {
                if (ChangeDirection <= 0)
                { // перемещение центра на указаный шаг в случайном направлении
                    rnd = rand.Next(0, 5);
                    ChangeDirection = Convert.ToInt16(rand.Next(300)/10.7);
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
