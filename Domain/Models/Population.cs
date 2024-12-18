namespace Domain.Models;

public class Population(int count)
{
    public readonly List<Individ> collection = [];
    
    private const int DefaultPopulationSize = 6;

    public event EventHandler? OnBornIndividual;

    public event EventHandler? OnDieIndividual;

    public Population() : this(DefaultPopulationSize)
    {
    }
    
    public void AddIndividual(Individ individ)
    {
        collection.Add(individ);
        OnBornIndividual?.Invoke(this, EventArgs.Empty);
    }
    
    public void RemoveIndividual(Individ individ)
    {
        collection.Remove(individ);
        OnDieIndividual?.Invoke(this, EventArgs.Empty);
    }

    public void InsertBatch(IEnumerable<Individ> individuals)
    {
        var enumerable = individuals as Individ[] ?? individuals.ToArray();
        collection.AddRange(enumerable);
        enumerable.ToList().ForEach(x => OnBornIndividual?.Invoke(this, EventArgs.Empty));
    }
    
    public void RemoveBatch(IEnumerable<Individ> individuals)
    {
        foreach (var individ in individuals)
        {
            collection.Remove(individ);
            OnDieIndividual?.Invoke(this, EventArgs.Empty);
        }
    }
}