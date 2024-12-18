namespace Domain.Models;

public class Population(int count)
{
    public readonly List<List<Individ>> Areas = Enumerable.Range(0, count)
        .Select(_ => new List<Individ>())
        .ToList();
    
    private const int DefaultPopulationSize = 6;

    public event EventHandler? OnBornIndividual;

    public event EventHandler? OnDieIndividual;

    public Population() : this(DefaultPopulationSize)
    {
    }

    public void AddIndividual(int areaCode, Individ individ)
    {
        if (areaCode > Areas.Count || areaCode < 0)
        {
            return;
        }

        Areas[areaCode].Add(individ);
        OnBornIndividual?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveIndividual(int areaCode, Individ individ)
    {
        if (areaCode > Areas.Count || areaCode < 0)
        {
            return;
        }

        Areas[areaCode].Remove(individ);
        OnDieIndividual?.Invoke(this, EventArgs.Empty);
    }
}