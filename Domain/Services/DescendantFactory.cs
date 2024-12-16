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
            Strategy.BaseGeneticAlgorithm => UseBaseGenetic(parent1, parent2, targetColor),
            _ => UseBaseGenetic(parent1, parent2, targetColor)
        };
    }

    private static Individual UseBaseGenetic(Individual parent1, Individual parent2, Color targetColor)
    {
        parent1.IsChecked = true;
        parent2.IsChecked = true;

        var chield = new Individual(new Point(), Color.White);
        // Кросинговер определяем точку разрыва
        var rand = new Random();
        var breakPoint = rand.Next(2, 30);
        var chield1 = new bool[Constants.ChromosomeSize];
        var chield2 = new bool[Constants.ChromosomeSize];
        for (var i = 0; i < breakPoint; i++) 
        {
            chield1[i] = parent1.Chromosome[i];
            chield2[i] = parent2.Chromosome[i];
        }
        for (var j = breakPoint; j < Constants.ChromosomeSize; j++) 
        {
            chield1[j] = parent2.Chromosome[j];
            chield2[j] = parent1.Chromosome[j];
        }
        // Мутация - замена случайного бита 
        // Задаем вероятность мутации для каждого из потомков
        double mutationProb = rand.Next(1, 10) / 10;// Вероятность мутации 
        var randDouble = new Random(rand.Next(1,100));
        var mutationProb1 = randDouble.NextDouble();
        var mutationProb2 = randDouble.NextDouble();
        // Задаем случайный номер бита для замены
        // не учитываем первый и последний
        var bit1 = rand.Next(0, 31);
        var bit2 = rand.Next(0, 31);
        if (mutationProb1 < mutationProb)
        {
            chield1[bit1] = !chield1[bit1];
        }
        if (mutationProb2 < mutationProb)
        {
            chield2[bit2] = !chield2[bit2];
        }
        //// проверка на живучость
        var num1 = Convert.ToInt32(Converter.FromBoolToColor(chield1).ToArgb());
        var num2 = Convert.ToInt32(Converter.FromBoolToColor(chield2).ToArgb());
        // Целевая функция близость кода цвета к коду цвета региона в котором находятся родители
        var regionColor = targetColor.ToArgb();
        if (Math.Abs(num1 - regionColor) < Math.Abs(num2 - regionColor))
        {
            for (var i = 0; i < 32; i++)
                chield.Chromosome[i] = chield1[i];
            chield.BitesColor = num1;
            chield.Center = parent1.Center;
            chield.Center.X += 30;
            chield.Center.Y += 10;
            var col = Converter.FromBoolToColor(chield1);
            chield.Pen.Color = col;
            chield.ColorOfInd = col;
        }
        else
        {
            for (var i = 0; i < 32; i++)
                chield.Chromosome[i] = chield2[i];
            chield.BitesColor = num2;
            chield.Center = parent2.Center;
            chield.Center.X += 30;
            chield.Center.Y += 10;
            var col = Converter.FromBoolToColor(chield2);
            chield.Pen.Color = col;
            chield.ColorOfInd = col;
        }
        return chield; 
    }
}