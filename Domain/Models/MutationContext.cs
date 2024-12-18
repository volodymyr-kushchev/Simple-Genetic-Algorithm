namespace Domain.Models;

public record MutationContext(Individual Parent1, Individual Parent2, bool[] Child1, bool[] Child2, Individual Child);