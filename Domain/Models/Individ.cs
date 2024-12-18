using System.Drawing;
using Domain.Services;

namespace Domain.Models;

public class Individ
{
    public int LifeTime { get; set; }
    public bool IsChecked { get; set; }
    public const int Step = 10;
    public const int Size = 40;
    
    public Point Center;
    public Color ColorOfInd;
    public int BitesColor { get; set; }
    public readonly bool[] Chromosome = new bool[Constants.ChromosomeSize];

    private int _changeDirection;
    private int _directionNow;

    public Rectangle Rectangle;
    public Pen Pen;
    private readonly Size _s;
    
    private readonly IRandomProvider _random;

    public Individ(IRandomProvider randomProvider, Point center, Color color) 
        : this(randomProvider, center, color, 200)
    { }

    public Individ(IRandomProvider randomProvider, Point center, Color color, int lifetime)
    {
        ColorOfInd = color;
        LifeTime = lifetime;
        IsChecked = false;
        BitesColor = 0;
        Array.Copy(Converter.FromColorToBool(color), Chromosome, Constants.ChromosomeSize);
        Center = center;
        _s.Height = Size;
        _s.Width = Size;
        Rectangle = new Rectangle(Center, _s);
        Pen = new Pen(color, Size);
        _changeDirection = randomProvider.Next(3, 25);
        
        _random = randomProvider;
    }
    
    public void SetCenter(Point p)
    {
        Center = p;
        Rectangle = new Rectangle(Center, _s);
    }

    public void Move()
    {
        if (LifeTime == 0) return;

        int rnd;

        if (_changeDirection <= 0)
        {
            // перемещение центра на указаный шаг в случайном направлении
            rnd = _random.Next(0, 5);
            _changeDirection = Convert.ToInt16(_random.Next(0, 300) / 10.7);
        }
        else
        {
            _changeDirection--;
            rnd = _directionNow;
        }

        switch (rnd)
        {
            case 1:
            {
                this.Center.X -= Step;
                _directionNow = rnd;
                break;
            }
            case 2:
            {
                this.Center.X += Step;
                _directionNow = rnd;
                break;
            }
            case 3:
            {
                this.Center.Y += Step;
                _directionNow = rnd;
                break;
            }
            case 4:
            {
                this.Center.Y -= Step;
                _directionNow = rnd;
                break;
            }
        }

        if (Center.X > 900)
            _directionNow = 1;
        if (Center.X < 0)
            _directionNow = 2;
        if (Center.Y > 500)
            _directionNow = 4;
        if (Center.Y < 0)
            _directionNow = 3;
        
        Rectangle = new Rectangle(Center, _s);
    }
}