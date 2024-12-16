using System.Drawing;
using Domain.Models;

namespace Domain.Services;

public interface IDescendantFactory
{
    Individual GenerateDescendant(Individual parent1, Individual parent2, Color targetColor);
}