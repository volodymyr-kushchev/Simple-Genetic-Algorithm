using System.Drawing;
using Domain.Models;

namespace Domain.Services;

public interface IDescendantFactory
{
    Individ GenerateDescendant(Individ parent1, Individ parent2, Color targetColor);
}