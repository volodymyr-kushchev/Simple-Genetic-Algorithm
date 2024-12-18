using System.Drawing;
using Domain.Services;

namespace Domain.Models;

public class Individ
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

    public Individ(Point center, Color color)
    {
        ColorOfInd = color;
        LifeTime = 200;
        IsChecked = false;
        BitesColor = 0;
        Array.Copy(Converter.FromColorToBool(color), Chromosome, ChromosomeSize);
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