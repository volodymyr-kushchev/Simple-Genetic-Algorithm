using System.Drawing;
using Domain.Models;

namespace Domain.Services;

public interface IIndividLifecycleService
{
    void EvaluateLifeStatus(Population population, Func<Point, Color> colorOfRegion);
}