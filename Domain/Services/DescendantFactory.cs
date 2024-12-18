using System.Drawing;
using Domain.Model;
using Domain.Models;

namespace Domain.Services;

public class DescendantFactory : IDescendantFactory
{
    private const Strategy Strategy = Model.Strategy.BaseGeneticAlgorithm;

    public Individual GenerateDescendant(Individual parent1, Individual parent2, Color targetColor)
    {
        return Strategy switch
        {
            Strategy.BaseGeneticAlgorithm => ApplyMutation(parent1, parent2, targetColor),
            _ => ApplyMutation(parent1, parent2, targetColor)
        };
    }
    
    private static Individual ApplyMutation(Individual parent1, Individual parent2, Color targetColor)
    {
        var mutation = InitializeMutation(parent1, parent2);

        mutation.Parent1.IsChecked = true;
        mutation.Parent2.IsChecked = true;
        // Кросинговер определяем точку разрыва
        CrossingOver(mutation);
        // Мутация - замена случайного бита
        Mutation(mutation);
        // проверка на живучость
        SurvivabilityTest(mutation, targetColor);

        return mutation.Child;
    }

    private static void CrossingOver(MutationContext mutation)
    {
        var rand = new Random();
        var breakPoint = rand.Next(2, Constants.ChromosomeSize - 2);
        for (var i = 0; i < breakPoint; i++)
        {
            mutation.Child1[i] = mutation.Parent1.Chromosome[i];
            mutation.Child2[i] = mutation.Parent2.Chromosome[i];
        }

        for (var j = breakPoint; j < Constants.ChromosomeSize; j++)
        {
            mutation.Child1[j] = mutation.Parent2.Chromosome[j];
            mutation.Child2[j] = mutation.Parent1.Chromosome[j];
        }
    }

    private static void Mutation(MutationContext mutation)
    {
        var rand = new Random();
        var mutationProb = rand.Next(1, 10) / 10.0; // Вероятность мутации 
        var randDouble = new Random(rand.Next(1, 100));
        var mutationProb1 = randDouble.NextDouble();
        var mutationProb2 = randDouble.NextDouble();
        // Задаем случайный номер бита для замены
        var bit1 = rand.Next(0, Constants.ChromosomeSize - 1);
        var bit2 = rand.Next(0, Constants.ChromosomeSize - 1);
        if (mutationProb1 < mutationProb)
        {
            mutation.Child1[bit1] = !mutation.Child1[bit1];
        }

        if (mutationProb2 < mutationProb)
        {
            mutation.Child1[bit2] = !mutation.Child1[bit2];
        }
    }

    private static void SurvivabilityTest(MutationContext mutation, Color targetColor)
    {
        var num1 = Convert.ToInt32(Converter.FromBoolToColor(mutation.Child1).ToArgb());
        var num2 = Convert.ToInt32(Converter.FromBoolToColor(mutation.Child2).ToArgb());
        // Целевая функция близость кода цвета к коду цвета региона в котором находятся родители
        var regionColor = targetColor.ToArgb();
        if (Math.Abs(num1 - regionColor) < Math.Abs(num2 - regionColor))
        {
            for (var i = 0; i < 32; i++)
                mutation.Child.Chromosome[i] = mutation.Child1[i];
            mutation.Child.BitesColor = num1;
            mutation.Child.Center = mutation.Parent1.Center;
            mutation.Child.Center.X += 30;
            mutation.Child.Center.Y += 10;
            var col = Converter.FromBoolToColor(mutation.Child1);
            mutation.Child.Pen.Color = col;
            mutation.Child.ColorOfInd = col;
        }
        else
        {
            for (var i = 0; i < 32; i++)
                mutation.Child.Chromosome[i] = mutation.Child2[i];
            mutation.Child.BitesColor = num2;
            mutation.Child.Center = mutation.Parent2.Center;
            mutation.Child.Center.X += 30;
            mutation.Child.Center.Y += 10;
            var col = Converter.FromBoolToColor(mutation.Child2);
            mutation.Child.Pen.Color = col;
            mutation.Child.ColorOfInd = col;
        }
    }

    private static MutationContext InitializeMutation(Individual parent1, Individual parent2) =>
        new MutationContext(
            parent1,
            parent2,
            new bool[Constants.ChromosomeSize],
            new bool[Constants.ChromosomeSize],
            new Individual(new Point(), Color.White));
}