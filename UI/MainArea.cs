using Domain.Models;
using Domain.Services;
using Serilog;

namespace UI;

public partial class MainArea : Form
{
    private readonly Population _population = new Population();
    private readonly Graphics _sheet;
    
    private readonly Dictionary<int, Color> _coloredAreas = new Dictionary<int, Color>();
    private readonly ILogger _logger;

    private readonly object _locker = new object();

    private readonly IIndividualLifecycleService _individualLifecycleService;

    public MainArea(ILogger logger, IIndividualLifecycleService individualLifecycleService)
    {
        InitializeComponent();
        PreSeed();
        Evolution();
        InitializeTick();

        _sheet = this.CreateGraphics();
        _logger = logger;
        _individualLifecycleService = individualLifecycleService;
    }

    private void Evolution()
    {
        _population.OnDieIndividual += (obj, arg) => { _logger?.Information("One individual has died"); };
        _population.OnBornIndividual += (obj, arg) => { _logger?.Information("One individual has born"); };
    }

    private void InitializeTick()
    {
        var timer = new System.Windows.Forms.Timer();
        timer.Interval = 45;

        timer.Tick += UpdatePopulation!;

        timer.Start();

        var timerForEvolution = new System.Windows.Forms.Timer();
        timerForEvolution.Interval = 1000;

        timerForEvolution.Tick += (sender, e) => { Task.Run(EvaluatePopulation); };
        timerForEvolution.Tick += UpdatePopulation!;

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
            var ind = new Individ(new Point(i * 10 + 100, i * 10 + 300), _coloredAreas[0]);
            lst1.Add(ind);
        }

        _population.Areas[0] = lst1;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(new Point(i * 20 + 100, i * 20 + 300), _coloredAreas[1]);
            lst2.Add(ind);
        }

        _population.Areas[1] = lst2;

        for (var i = 0; i < 1; i++)
        {
            var ind = new Individ(new Point(i * 70 + 500, i * 35 + 200), _coloredAreas[2]);
            lst3.Add(ind);
        }

        _population.Areas[2] = lst3;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(new Point(i * 80 + 30, i * 60 + 300), _coloredAreas[3]);
            lst4.Add(ind);
        }

        _population.Areas[3] = lst4;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(new Point(i * 50 + 200, i * 40 + 160), _coloredAreas[4]);
            lst5.Add(ind);
        }

        _population.Areas[4] = lst5;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individ(new Point(i * 60 + 300, i * 70), _coloredAreas[5]);
            lst6.Add(ind);
        }

        _population.Areas[5] = lst6;
        lst6.Clear();
    }

    private void UpdatePopulation(object sender, EventArgs e)
    {
        lock (_locker)
        {
            foreach (var ind in _population.Areas.Cast<IEnumerable<Individ>>().SelectMany(pop => pop))
            {
                ind.Update();
                ind.LifeTime--;
                ind.IsChecked = false;
            }

            _sheet.Clear(Color.White);
            foreach (var ind in _population.Areas.Cast<IEnumerable<Individ>>().SelectMany(pop => pop))
            {
                _sheet.DrawRectangle(ind.Pen, ind.Rectangle);
            }
        }
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

    private void EvaluatePopulation()
    {
        lock (_locker)
        {
            _individualLifecycleService.EvaluateLifeStatus(_population, ColorOfRegion);
        }
    }
}