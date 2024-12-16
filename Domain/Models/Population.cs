namespace Domain.Models;

public class Population(int count)
{
    public readonly List<List<Individual>> Areas = Enumerable.Range(0, count)
        .Select(_ => new List<Individual>())
        .ToList();
    
    private const int DefaultPopulationSize = 6;

    public event EventHandler? OnBornIndividual;

    public event EventHandler? OnDieIndividual;

    public Population() : this(DefaultPopulationSize)
    {
    }

    public void AddIndividual(int areaCode, Individual individual)
    {
        if (areaCode > Areas.Count || areaCode < 0)
        {
            return;
        }

        Areas[areaCode].Add(individual);
        OnBornIndividual?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveIndividual(int areaCode, Individual individual)
    {
        if (areaCode > Areas.Count || areaCode < 0)
        {
            return;
        }

        Areas[areaCode].Remove(individual);
        OnDieIndividual?.Invoke(this, EventArgs.Empty);
    }
}