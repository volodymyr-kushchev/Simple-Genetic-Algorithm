using System.Diagnostics.CodeAnalysis;

namespace Domain.Services;

[ExcludeFromCodeCoverage]
public class RandomProvider : IRandomProvider
{
    public int Next(int minValue, int maxValue)
    {
        return Random.Shared.Next(minValue, maxValue);
    }
}