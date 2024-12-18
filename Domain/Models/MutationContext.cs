namespace Domain.Models;

public record MutationContext(Individ Parent1, Individ Parent2, bool[] Child1, bool[] Child2, Individ Child);