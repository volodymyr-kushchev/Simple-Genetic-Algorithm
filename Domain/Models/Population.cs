namespace Domain.Models;

public class Population(int count)
{
    public readonly List<Individ> Collection = [];
    
    private const int DefaultPopulationSize = 6;

    public event EventHandler? OnBornIndividual;

    public event EventHandler? OnDieIndividual;

    public Population() : this(DefaultPopulationSize)
    {
    }
    
    public void AddIndividual(Individ individ)
    {
        if (Collection.Count >= Constants.MaxPopulationSize)
        {
            OnDieIndividual?.Invoke(this, EventArgs.Empty);
            return;
        }
        Collection.Add(individ);
        OnBornIndividual?.Invoke(this, EventArgs.Empty);
    }
    
    public void RemoveIndividual(Individ individ)
    {
        Collection.Remove(individ);
        OnDieIndividual?.Invoke(this, EventArgs.Empty);
    }

    public void InsertBatch(IEnumerable<Individ> individuals)
    {
        var enumerable = individuals as Individ[] ?? individuals.ToArray();
        if (Collection.Count + enumerable.Length >= Constants.MaxPopulationSize)
        {
            enumerable.ToList().ForEach(x => OnDieIndividual?.Invoke(this, EventArgs.Empty));
            return;
        }
        
        Collection.AddRange(enumerable);
        enumerable.ToList().ForEach(x => OnBornIndividual?.Invoke(this, EventArgs.Empty));
    }
    
    public void RemoveBatch(IEnumerable<Individ> individuals)
    {
        foreach (var individ in individuals)
        {
            Collection.Remove(individ);
            OnDieIndividual?.Invoke(this, EventArgs.Empty);
        }
    }
}