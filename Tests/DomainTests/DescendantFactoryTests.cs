using System.Drawing;
using Domain.Model;
using Domain.Models;
using Domain.MutationStrategies;
using Domain.Services;
using FluentAssertions;
using Moq;

namespace Tests.DomainTests;

public class DescendantFactoryTests
{
    [Fact]
    public void DecendantFactory_GenerateDecendants_ShouldReturnAllDescendants()
    {
        // arrange
        var mockRandomProvider = new Mock<IRandomProvider>();
        var descendantFactory = new DescendantFactory(mockRandomProvider.Object);
        var parent1 = new Individ(mockRandomProvider.Object, new Point(100, 300), Color.Aqua);
        var parent2 = new Individ(mockRandomProvider.Object, new Point(100, 300), Color.Gold);
        var expectedPoint = new Point(130, 310);
        
        // act
        var result = descendantFactory.GenerateDescendant(parent1, parent2, Color.Aqua);
        
        // assert
        result.Center.Should().Be(expectedPoint);
    }
    
    [Fact]
    public void DecendantFactory_SetStrategy_ShouldCreateNewOne()
    {
        // arrange
        var mockRandomProvider = new Mock<IRandomProvider>();
        var descendantFactory = new DescendantFactory(mockRandomProvider.Object);
        descendantFactory.SetMutationStrategy(Strategy.BaseGeneticAlgorithm);
        
        // act
        var strategy = descendantFactory.GetStrategy();
        
        // assert
        strategy.Should().BeOfType(typeof(GenericAlgoWithColorArea));
    }
}