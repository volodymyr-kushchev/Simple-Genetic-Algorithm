using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Domain.Models;
using Domain.Services;
using Serilog;

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
    public partial class MainWindow : Window
    {
        private readonly Population _population = new Population();
        private readonly Dictionary<int, SolidColorBrush> _coloredAreas = new();
        private readonly ILogger _logger;
        private readonly object _locker = new();

        private readonly IIndividLifecycleService _individLifecycleService;
        private readonly IRandomProvider _randomProvider;

        public MainWindow(ILogger logger, IIndividLifecycleService individLifecycleService, IRandomProvider randomProvider)
        {
            InitializeComponent();

            _logger = logger;
            _individLifecycleService = individLifecycleService;
            _randomProvider = randomProvider;

            PreSeed();
            Evolution();
            InitializeTick();
        }

        private void Evolution()
        {
            _population.OnDieIndividual += (obj, arg) => { _logger?.Information("One individual has died"); };
            _population.OnBornIndividual += (obj, arg) => { _logger?.Information("One individual has born"); };
        }

        private void InitializeTick()
        {
            var updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(45) };
            updateTimer.Tick += UpdatePopulation;
            updateTimer.Start();

            var evolutionTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            evolutionTimer.Tick += (sender, e) => { Task.Run(EvaluatePopulation); };
            evolutionTimer.Tick += UpdatePopulation;
            evolutionTimer.Start();
        }

        private void PreSeed()
        {
            _coloredAreas.Add(0, Brushes.Red);
            _coloredAreas.Add(1, Brushes.SlateGray);
            _coloredAreas.Add(2, Brushes.Black);
            _coloredAreas.Add(3, Brushes.Green);
            _coloredAreas.Add(4, Brushes.Gold);
            _coloredAreas.Add(5, Brushes.Yellow);

            var lists = new List<List<Individ>>
            {
                CreateBatch(2, 0, (i) => new Point(i * 10 + 100, i * 10 + 300)),
                CreateBatch(2, 1, (i) => new Point(i * 20 + 100, i * 20 + 300)),
                CreateBatch(1, 2, (i) => new Point(i * 70 + 500, i * 35 + 200)),
                CreateBatch(2, 3, (i) => new Point(i * 80 + 30, i * 60 + 300)),
                CreateBatch(2, 4, (i) => new Point(i * 50 + 200, i * 40 + 160)),
                CreateBatch(2, 5, (i) => new Point(i * 60 + 300, i * 70)),
            };

            foreach (var lst in lists)
            {
                _population.InsertBatch(lst);
            }
        }

        private List<Individ> CreateBatch(int count, int colorIndex, Func<int, Point> pointGenerator)
        {
            var list = new List<Individ>();
            // need to convernt System.Windows.Point to System.Drawing.Point
            // for (var i = 0; i < count; i++)
            // {
            //     list.Add(new Individ(_randomProvider, pointGenerator(i), _coloredAreas[colorIndex]));
            // }
            return list;
        }

        private void UpdatePopulation(object? sender, EventArgs e)
        {
            lock (_locker)
            {
                foreach (var ind in _population.collection)
                {
                    ind.Move();
                    ind.LifeTime--;
                    ind.IsChecked = false;
                }

                DrawPopulation();
            }
        }

        private void DrawPopulation()
        {
            var drawingVisual = new DrawingVisual();
            using (var dc = drawingVisual.RenderOpen())
            {
                foreach (var ind in _population.collection)
                {
                    var rect = new Rect(ind.Rectangle.X, ind.Rectangle.Y, ind.Rectangle.Width, ind.Rectangle.Height);
                    // brash the same story windows to drawing conversion
                    // dc.DrawRectangle(ind.Pen.Brush, null, rect);
                }
            }
            DrawingCanvas.Children.Clear();
            // some error.
            // var hostVisual = new FrameworkElement
            // {
            //     Visual = drawingVisual
            // };
            // DrawingCanvas.Children.Add(hostVisual);
        }

        private Color ColorOfRegion(Point p)
        {
            if (p.X > 250)
            {
                if (p.Y > 300)
                {
                    return p.Y < 600 ? _coloredAreas[4].Color : _coloredAreas[5].Color;
                }
                else
                {
                    return _coloredAreas[3].Color;
                }
            }
            else
            {
                if (p.Y > 300)
                {
                    return p.Y < 600 ? _coloredAreas[1].Color : _coloredAreas[2].Color;
                }
                else
                {
                    return _coloredAreas[0].Color;
                }
            }
        }

        private void EvaluatePopulation()
        {
            lock (_locker)
            {
                // same story with conversion of color from one lib to another.
                // _individLifecycleService.EvaluateLifeStatus(_population, ColorOfRegion);
            }
        }
    }