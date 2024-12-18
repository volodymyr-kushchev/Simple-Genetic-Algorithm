namespace Domain.Services;

public class RandomProvider : IRandomProvider
{
    public int Next(int minValue, int maxValue)
    {
        return Random.Shared.Next(minValue, maxValue);
    }
}