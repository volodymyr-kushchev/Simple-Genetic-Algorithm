using Domain;
using Infrastructure;
using Serilog;
using Serilog.Core;

namespace UI;

public partial class MainArea : Form
{
    private readonly Population _population = new Population();
    private readonly Graphics _sheet;

    // TODO: move to constant
    private readonly Color[] _colorArea = new Color[6];
    private Logger? _logger;

    private readonly object _locker = new object();

    public MainArea()
    {
        InitializeLogger();
        InitializeComponent();
        _sheet = this.CreateGraphics();
        PreSeed();
        Evolution();
        InitializeTick();
    }

    private void Evolution()
    {
        _population.OnDieIndividual += (obj, arg) =>
        {
            _logger?.Information("One individual has died");
        };

        _population.OnBornIndividual += (obj, arg) =>
        {
            _logger?.Information("One individual has born");
        };
    }

    private void InitializeTick()
    {
        var timer = new System.Windows.Forms.Timer();
        timer.Interval = 45;

        timer.Tick += UpdatePopulation!;

        timer.Start();

        var timerForEvolution = new System.Windows.Forms.Timer();
        timerForEvolution.Interval = 1000;

        timerForEvolution.Tick += (sender, e) => { Task.Run(CheckNew); };

        timerForEvolution.Start();
    }

    private void InitializeLogger()
    {
        var filePath = Directory.GetCurrentDirectory();
        filePath = Path.Combine(filePath, "logs.txt");

        _logger = new LoggerConfiguration()
            .WriteTo.File(filePath)
            .CreateLogger();

        LogWatcher.WatchLogs(filePath);
    }

    private void PreSeed()
    {
        var lst1 = new List<Individual>();
        List<Individual> lst2 = [];
        List<Individual> lst3 = [];
        List<Individual> lst4 = [];
        List<Individual> lst5 = [];
        List<Individual> lst6 = [];
        _colorArea[0] = Color.Red;
        _colorArea[1] = Color.SlateGray;
        _colorArea[2] = Color.Black;
        _colorArea[3] = Color.Green;
        _colorArea[4] = Color.Gold;
        _colorArea[5] = Color.Yellow;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individual(new Point(i * 10 + 100, i * 10 + 300), Color.Red);
            lst1.Add(ind);
        }
        _population.Areas[0] = lst1;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individual(new Point(i * 20 + 100, i * 20 + 300), Color.SlateGray);
            lst2.Add(ind);
        }
        _population.Areas[1] = lst2;

        for (var i = 0; i < 1; i++)
        {
            var ind = new Individual(new Point(i * 70 + 500, i * 35 + 200), Color.Black);
            lst3.Add(ind);
        }
        _population.Areas[2] = lst3;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individual(new Point(i * 80 + 30, i * 60 + 300), Color.Green);
            lst4.Add(ind);
        }
        _population.Areas[3] = lst4;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individual(new Point(i * 50 + 200, i * 40 + 160), Color.Gold);
            lst5.Add(ind);
        }
        _population.Areas[4] = lst5;
        for (var i = 0; i < 2; i++)
        {
            var ind = new Individual(new Point(i * 60 + 300, i * 70), Color.Yellow);
            lst6.Add(ind);
        }
        _population.Areas[5] = lst6;
        lst6.Clear();
    }

    private void UpdatePopulation(object sender, EventArgs e)
    {
        lock (_locker)
        {
            _sheet.Clear(Color.White);
            foreach (IEnumerable<Individual> pop in _population.Areas)
            {
                foreach (var ind in pop)
                {
                    ind.Update();
                    ind.LifeTime--;
                    _sheet.DrawRectangle(ind.Pen, ind.Rectangle);
                    ind.IsChecked = false;
                }
            }
        }
    }

    private Color ColorOfRegion(Point p)
    {
        if (p.X > 250)
        {
            if (p.Y > 300)
            {
                if (p.Y < 600) return _colorArea[4];
                else return _colorArea[5];
            }
            else { return _colorArea[3]; }
        }
        else
        {
            if (p.Y > 300)
            {
                if (p.Y < 600) return _colorArea[1];
                else return _colorArea[2];
            }
            else { return _colorArea[0]; }
        }
    }

    private void CheckNew()
    {
        lock (_locker)
        {
            for (var i = 0; i < _population.Areas.Count; i++)
            {
                var ind = _population.Areas[i].Count > 0 ? _population.Areas[i][0] : null;
                for (var j = 0; j < _population.Areas[i].Count; j++)
                {
                    if (_population.Areas[i][j].LifeTime < 0)
                    {
                        var ind2 = _population.Areas[i][j];
                        _population.RemoveIndividual(i, ind2);
                    }
                    else
                    {
                        if (!_population.Areas[i][j].IsChecked && ind is not null)
                            if (Math.Sqrt(Math.Pow(ind.Center.X - _population.Areas[i][j].Center.X, 2) + Math.Pow(ind.Center.Y - _population.Areas[i][j].Center.Y, 2)) < ind.Size)
                            {
                                var some = ind.GenerateDescendant(ind, _population.Areas[i][j], ColorOfRegion(ind.Center));
                                _population.AddIndividual(i, some);
                            }
                    }
                }
                if (ind != null)
                {
                    ind.IsChecked = true;
                }
            }
        }
    }
}