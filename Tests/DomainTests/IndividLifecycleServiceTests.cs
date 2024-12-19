using System.Drawing;
using Domain.Models;
using Domain.Services;
using FluentAssertions;
using Moq;

namespace Tests.DomainTests;

public class IndividLifecycleServiceTests
{
    [Fact]
    public void IndividLifecycleServiceTests_TwoParentsIntersect_NewIndividBorn()
    {
        // arrange
        var mockDecendantFactory = new Mock<IDescendantFactory>();
        var mockRandomGenerator = new Mock<IRandomProvider>();
        var individLifecycleService = new IndividLifecycleService(mockDecendantFactory.Object);
        var population = new Population();
        population.AddIndividual(new Individ(mockRandomGenerator.Object, new Point(100, 300), Color.Aqua));
        population.AddIndividual(new Individ(mockRandomGenerator.Object, new Point(100, 300), Color.Gold));
        
        // act
        individLifecycleService.EvaluateLifeStatus(population, (point) => Color.Azure);
        
        // assert
        population.Collection.Count.Should().Be(3);
    }

    [Fact]
    public void IndividLifecycleServiceTests_NoIntersection_NoChangesInPopulation()
    {
        // arrange
        var mockDecendantFactory = new Mock<IDescendantFactory>();
        var mockRandomGenerator = new Mock<IRandomProvider>();
        var individLifecycleService = new IndividLifecycleService(mockDecendantFactory.Object);
        var population = new Population();
        population.AddIndividual(new Individ(mockRandomGenerator.Object, new Point(100, 300), Color.Aqua));
        population.AddIndividual(new Individ(mockRandomGenerator.Object, new Point(100, 100), Color.Gold));
        
        // act
        individLifecycleService.EvaluateLifeStatus(population, (point) => Color.Azure);
        
        // assert
        population.Collection.Count.Should().Be(2);
    }
    
    [Fact]
    public void IndividLifecycleServiceTests_IndividDied_PopulationDecreased()
    {
        // arrange
        var mockDecendantFactory = new Mock<IDescendantFactory>();
        var mockRandomGenerator = new Mock<IRandomProvider>();
        var individLifecycleService = new IndividLifecycleService(mockDecendantFactory.Object);
        var population = new Population();
        population.AddIndividual(new Individ(mockRandomGenerator.Object, new Point(100, 300), Color.Aqua, -1));
        population.AddIndividual(new Individ(mockRandomGenerator.Object, new Point(100, 100), Color.Gold));
        
        // act
        individLifecycleService.EvaluateLifeStatus(population, (point) => Color.Azure);
        
        // assert
        population.Collection.Count.Should().Be(1);
    }
}