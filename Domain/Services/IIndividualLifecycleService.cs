using System.Drawing;
using Domain.Models;

namespace Domain.Services;

public interface IIndividualLifecycleService
{
    void EvaluateLifeStatus(Population population, Func<Point, Color> colorOfRegion);
}