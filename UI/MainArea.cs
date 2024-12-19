using Domain.Models;
using Domain.Services;
using Serilog;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace UI;

public partial class MainArea : Form
{
    private readonly Population _population = new();
    
    private readonly Dictionary<int, Color> _coloredAreas = new();
    private readonly ILogger _logger;

    private readonly object _locker = new();

    private readonly IIndividLifecycleService _individLifecycleService;
    private readonly IRandomProvider _randomProvider;
    
    private SKControl _skiaControl;

    public MainArea(ILogger logger, IIndividLifecycleService individLifecycleService, IRandomProvider randomProvider)
    {
        InitializeComponent();
        
        _logger = logger;
        _individLifecycleService = individLifecycleService;
        _randomProvider = randomProvider;

        SetupSkiaControl();
        PreSeed();
        Evolution();
        InitializeTick();
    }
    
    private void SetupSkiaControl()
    {
        _skiaControl = new SKControl
        {
            Dock = DockStyle.Fill
        };

        _skiaControl.PaintSurface += OnPaintSurface!;
        this.Controls.Add(_skiaControl);
    }
    
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        lock (_locker)
        {
            foreach (var ind in _population.collection)
            {
                var paint = new SKPaint
                {
                    Color = ind.ColorOfInd.ToSkiaColor(),
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 2
                };

                canvas.DrawRect(ind.Rectangle.ToSkiaRect(), paint);
            }
        }
    }

    private void InitializeTick()
    {
        var timer = new System.Windows.Forms.Timer { Interval = 45 };
        timer.Tick += UpdatePopulation!;
        timer.Start();
        
        var timerForEvolution = new System.Windows.Forms.Timer { Interval = 1000 };
        timerForEvolution.Tick += EvaluatePopulation!;
        timerForEvolution.Start();
    }

    private void PreSeed()
    {
        var lst1 = new List<Individ>();
        List<Individ> lst2 = [];
        List<Individ> lst3 = [];
        List<Individ> lst4 = [];
        List<Individ> lst5 = [];
        List<Individ> lst6 = [];
        _coloredAreas.Add(0, Color.Red);
        _coloredAreas.Add(1, Color.SlateGray);
        _coloredAreas.Add(2, Color.Black);
        _coloredAreas.Add(3, Color.Green);
        _coloredAreas.Add(4, Color.Gold);
        _coloredAreas.Add(5, Color.Yellow);

        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(_randomProvider, new Point(i * 10 + 100, i * 10 + 300), _coloredAreas[0]);
            lst1.Add(ind);
        }
        
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(_randomProvider, new Point(i * 20 + 100, i * 20 + 300), _coloredAreas[1]);
            lst2.Add(ind);
        }

        for (var i = 0; i < 1; i++)
        {
            var ind = new Individ(_randomProvider, new Point(i * 70 + 500, i * 35 + 200), _coloredAreas[2]);
            lst3.Add(ind);
        }
        
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(_randomProvider, new Point(i * 80 + 30, i * 60 + 300), _coloredAreas[3]);
            lst4.Add(ind);
        }
        
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(_randomProvider, new Point(i * 50 + 200, i * 40 + 160), _coloredAreas[4]);
            lst5.Add(ind);
        }
        
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(_randomProvider, new Point(i * 60 + 300, i * 70), _coloredAreas[5]);
            lst6.Add(ind);
        }
        
        _population.InsertBatch(lst1);
        _population.InsertBatch(lst2);
        _population.InsertBatch(lst3);
        _population.InsertBatch(lst4);
        _population.InsertBatch(lst5);
        _population.InsertBatch(lst6);
    }

    private void UpdatePopulation(object sender, EventArgs e)
    {
        lock (_locker)
        {
            _logger?.Information(_population.collection.Count().ToString());
            foreach (var ind in _population.collection)
            {
                ind.Move();
                ind.LifeTime--;
                ind.IsChecked = false;
            }
        }
        
        _skiaControl.Invalidate();
    }

    private Color ColorOfRegion(Point p)
    {
        if (p.X > 250)
        {
            if (p.Y > 300)
            {
                return p.Y < 600 ? _coloredAreas[4] : _coloredAreas[5];
            }
            else
            {
                return _coloredAreas[3];
            }
        }
        else
        {
            if (p.Y > 300)
            {
                return p.Y < 600 ? _coloredAreas[1] : _coloredAreas[2];
            }
            else
            {
                return _coloredAreas[0];
            }
        }
    }

    private void EvaluatePopulation(object sender, EventArgs e)
    {
        lock (_locker)
        {
            _individLifecycleService.EvaluateLifeStatus(_population, ColorOfRegion);
        }
    }
    
    private void Evolution()
    {
        _population.OnDieIndividual += (obj, arg) => { _logger?.Information("One individual has died"); };
        _population.OnBornIndividual += (obj, arg) => { _logger?.Information("One individual has born"); };
    }
}