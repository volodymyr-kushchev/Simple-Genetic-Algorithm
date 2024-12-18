using System.Drawing;
using Domain.Models;

namespace Domain.MutationStrategies;

public interface IGenericStrategy
{
    Individ ApplyMutation(Individ parent1, Individ parent2, Color targetColor);
}