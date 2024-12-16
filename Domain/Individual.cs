using System.Drawing;

namespace Domain
{
    public class Individual
    {
        const int ChromosomeSize = 32;
        public int LifeTime { get; set; }
        public bool IsChecked { get; set; }
        public int Step = 10;
        public int Size = 40;
        public Point Center;

        public Color ColorOfInd;
        public Int32 BitesColor { get; set; }
        public bool[] Chromosome = new bool[ChromosomeSize];

        public int ChangeDirection;
        public int DirectionNow;

        public Rectangle Rectangle;
        public Pen Pen;
        public Size S;

        public Individual(Point center, Color color)
        {
            ColorOfInd = color;
            LifeTime = 200;
            IsChecked = false;
            BitesColor = 0;
            Array.Copy(FromColorToBool(color), Chromosome, ChromosomeSize);
            Center = center;
            S.Height = Size;
            S.Width = Size;
            Rectangle = new Rectangle(Center, S);
            Pen = new Pen(color, Size);
            Random rnd = new Random();
            ChangeDirection = rnd.Next(3,25);
        }
        public void SetCenter(Point p)
        {
            Center = p;
            Rectangle = new Rectangle(Center, S);
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
            bool[] res = new bool[ChromosomeSize];
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

            foreach(bool chroma in Chromosome)
            {
                res += chroma ? "1" : "0";
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


        public Individual GenerateDescendant(Individual parent1, Individual parent2, Color targetColor)
        {
            parent1.IsChecked = true;
            parent2.IsChecked = true;

            Individual chield = new Individual(new Point(), Color.White);
            // Кросинговер определяем точку разрыва
            Random rand = new Random();
            int breakPoint = rand.Next(2, 30);
            bool[] chield1 = new bool[ChromosomeSize];
            bool[] chield2 = new bool[ChromosomeSize];
            for (int i = 0; i < breakPoint; i++) 
            {
                chield1[i] = parent1.Chromosome[i];
                chield2[i] = parent2.Chromosome[i];
            }
            for (int j = breakPoint; j < ChromosomeSize; j++) 
            {
                chield1[j] = parent2.Chromosome[j];
                chield2[j] = parent1.Chromosome[j];
            }
            // Мутация - замена случайного бита 
            // Задаем вероятность мутации для каждого из потомков
            double mutationProb = rand.Next(1, 10) / 10;// Вероятность мутации 
            Random randDouble = new Random(rand.Next(1,100));
            double mutationProb1 = randDouble.NextDouble();
            double mutationProb2 = randDouble.NextDouble();
            // Задаем случайный номер бита для замены
            // не учитываем первый и последний
            int bit1 = rand.Next(0, 31);
            int bit2 = rand.Next(0, 31);
            if (mutationProb1 < mutationProb)
            {
                chield1[bit1] = !chield1[bit1];
            }
            if (mutationProb2 < mutationProb)
            {
                chield2[bit2] = !chield2[bit2];
            }
            //// проверка на живучость
            Int32 num1 = Convert.ToInt32(FromBoolToColor(chield1).ToArgb());
            Int32 num2 = Convert.ToInt32(FromBoolToColor(chield2).ToArgb());
            // Целевая функция близость кода цвета к коду цвета региона в котором находятся родители
            Int32 regionColor = targetColor.ToArgb();
            if (Math.Abs(num1 - regionColor) < Math.Abs(num2 - regionColor))
            {
                for (int i = 0; i < 32; i++)
                    chield.Chromosome[i] = chield1[i];
                chield.BitesColor = num1;
                chield.Center = parent1.Center;
                chield.Center.X += 30;
                chield.Center.Y += 10;
                Color col = FromBoolToColor(chield1);
                chield.Pen.Color = col;
                chield.ColorOfInd = col;
            }
            else
            {
                for (int i = 0; i < 32; i++)
                    chield.Chromosome[i] = chield2[i];
                chield.BitesColor = num2;
                chield.Center = parent2.Center;
                chield.Center.X += 30;
                chield.Center.Y += 10;
                Color col = FromBoolToColor(chield2);
                chield.Pen.Color = col;
                chield.ColorOfInd = col;
            }
            return chield;
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
                Rectangle = new Rectangle(Center, S);
            }
        }
    }
}
