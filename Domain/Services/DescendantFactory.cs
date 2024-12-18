using System.Drawing;
using Domain.Model;
using Domain.Models;
using Domain.MutationStrategies;

namespace Domain.Services;

public class DescendantFactory : IDescendantFactory
{
    private Strategy _strategy = Strategy.BaseGeneticAlgorithm;
    private IGenericStrategy _genericStrategy;
    private readonly IRandomProvider _randomProvider;

    public DescendantFactory(IRandomProvider randomProvider)
    {
        _randomProvider = randomProvider;
        _genericStrategy = new GenericAlgoWithColorArea(_randomProvider);
    }

    public Individ GenerateDescendant(Individ parent1, Individ parent2, Color targetColor) =>
        _genericStrategy.ApplyMutation(parent1, parent2, targetColor);

    public void SetMutationStrategy(Strategy strategy)
    {
        _strategy = strategy;
        _genericStrategy = _strategy switch
        {
            Strategy.BaseGeneticAlgorithm => new GenericAlgoWithColorArea(_randomProvider),
            _ => new GenericAlgoWithColorArea(_randomProvider)
        };
    }
}