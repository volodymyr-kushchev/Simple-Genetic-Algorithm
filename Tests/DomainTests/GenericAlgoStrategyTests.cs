using System.Drawing;
using Domain.Models;
using Domain.MutationStrategies;
using Domain.Services;
using FluentAssertions;
using Moq;

namespace Tests.DomainTests;

public class GenericAlgoStrategyTests
{
    [Fact]
    public void GenericAlgoStrategy_GenerateChild_ShouldMoveCenter()
    {
        // arrange
        var mockRandomProvider = new Mock<IRandomProvider>();
        var strategy = new GenericAlgoWithColorArea(mockRandomProvider.Object);
        var parent1 = new Individ(mockRandomProvider.Object, new Point(100, 300), Color.Aqua);
        var parent2 = new Individ(mockRandomProvider.Object, new Point(100, 300), Color.Gold);
        var expectedPoint = new Point(130, 310);
        
        // act
        var result = strategy.ApplyMutation(parent1, parent2, Color.Aqua);
        
        // assert
        result.Center.Should().Be(expectedPoint);
    }
}