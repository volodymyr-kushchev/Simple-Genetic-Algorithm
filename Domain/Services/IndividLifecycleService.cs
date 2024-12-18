using System.Drawing;
using Domain.Models;

namespace Domain.Services;

public class IndividLifecycleService(IDescendantFactory descendantFactory) : IIndividLifecycleService
{
    public void EvaluateLifeStatus(Population population, Func<Point, Color> colorOfRegion)
    {
        population.RemoveBatch(population.collection.Where(x => x.LifeTime < 0).ToList());
        var toAppend = new List<Individ>();

        foreach (var individ in population.collection)
        {
            if (individ.IsChecked)
            {
                continue;
            }

            foreach (var neighbour in population.collection)
            {
                if (neighbour == individ) continue;
                
                if (!(Math.Sqrt(Math.Pow(individ.Center.X - neighbour.Center.X, 2) +
                                Math.Pow(individ.Center.Y - neighbour.Center.Y, 2)) <
                      Individ.Size)) continue;
                
                var child = descendantFactory.GenerateDescendant(individ, neighbour, colorOfRegion(individ.Center));
                toAppend.Add(child);
                
                individ.IsChecked = true;
                neighbour.IsChecked = true;
            }
            
            individ.IsChecked = true;
        }
        
        population.InsertBatch(toAppend);
    }
}