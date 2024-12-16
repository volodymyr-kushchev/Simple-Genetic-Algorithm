using System.Drawing;
using Domain.Models;

namespace Domain.Services;

public class IndividualLifecycleService(IDescendantFactory descendantFactory) : IIndividualLifecycleService
{
    public void EvaluateLifeStatus(Population population, Func<Point, Color> colorOfRegion)
    {
        for (var i = 0; i < population.Areas.Count; i++)
        {
            var ind = population.Areas[i].Count > 0 ? population.Areas[i][0] : null;
            for (var j = 0; j < population.Areas[i].Count; j++)
            {
                if (population.Areas[i][j].LifeTime < 0)
                {
                    var ind2 = population.Areas[i][j];
                    population.RemoveIndividual(i, ind2);
                }
                else
                {
                    if (!population.Areas[i][j].IsChecked && ind is not null)
                        if (Math.Sqrt(Math.Pow(ind.Center.X - population.Areas[i][j].Center.X, 2) +
                                      Math.Pow(ind.Center.Y - population.Areas[i][j].Center.Y, 2)) < ind.Size)
                        {
                            var some = descendantFactory.GenerateDescendant(ind, population.Areas[i][j],
                                colorOfRegion(ind.Center));
                            population.AddIndividual(i, some);
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