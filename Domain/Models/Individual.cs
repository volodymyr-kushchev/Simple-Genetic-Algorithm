using System.Drawing;
using Domain.Services;

namespace Domain.Models;

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
        var rnd = new Random();
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
        var res = new bool[32];
        bool[] b0 = new bool[8], b1 = new bool[8], b2 = new bool[8], b3 = new bool[8];
        for (var i = 0; i < 8; i++)
            b0[i] = arr[i];
        for (var i = 8; i < 16; i++)
            b1[(i - 8)] = arr[i];
        for (var i = 16; i < 24; i++)
            b2[(i - 16)] = arr[i];
        for (var i = 24; i < 32; i++)
            b3[(i - 24)] = arr[i];
        var n0 = Convert.ToInt32(Converter.ConvertBoolArrayToByte(b0));
        var n1 = Convert.ToInt32(Converter.ConvertBoolArrayToByte(b1));
        var n2 = Convert.ToInt32(Converter.ConvertBoolArrayToByte(b2));
        var n3 = Convert.ToInt32(Converter.ConvertBoolArrayToByte(b3));
        var color = Color.FromArgb(n0, n1, n2, n3);
        return color;

    }
    // Gets color and convert to bool array 
    // Returns bool array
    public bool[] FromColorToBool(Color color)
    {
        var res = new bool[ChromosomeSize];
        var b0 = Converter.ConvertByteToBoolArray(color.A);
        for (var i = 0; i < 8; i++)
            res[i] = b0[i];
        var b1 = Converter.ConvertByteToBoolArray(color.R);
        for (var i = 8; i < 16; i++)
            res[i] = b1[(i - 8)];
        var b2 = Converter.ConvertByteToBoolArray(color.G);
        for (var i = 16; i < 24; i++)
            res[i] = b2[(i - 16)];
        var b3 = Converter.ConvertByteToBoolArray(color.B);
        for (var i = 24; i < 32; i++)
            res[i] = b3[(i - 24)];
        return res;

    }
    
    public void Update()
    {
        int rnd;
        var rand = new Random();
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