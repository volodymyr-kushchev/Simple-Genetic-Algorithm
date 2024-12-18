using System.Drawing;
using Domain.Models;
using Domain.Services;
using FluentAssertions;
using Moq;

namespace Tests.DomainTests;

public class IndividTests
{
    [Fact]
    public void Individ_LifetimeZero_ShouldNotMove()
    {
        // arrange
        var mockRandomGenerator = new Mock<IRandomProvider>().Object;
        var initialPosition = new Point(100, 300);
        var individ = new Individ(mockRandomGenerator, initialPosition, Color.Aqua);
        individ.LifeTime = 0;
        
        // act
        individ.Move();

        // assert
        individ.Center.Should().Be(initialPosition);
    }
    
    [Theory]
    [InlineData(510, 500)]
    [InlineData(450, 450)]
    public void Individ_ReachedTopBorder_ShouldMoveDown(int intialPositionY, int reslutPositionY)
    {
        // arrange
        var mockRandomGenerator = new Mock<IRandomProvider>();
        mockRandomGenerator.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(10);
        var initialPosition = new Point(100, intialPositionY);
        var individ = new Individ(mockRandomGenerator.Object, initialPosition, Color.Aqua);
        
        // act
        individ.Move();
        individ.Move();

        // assert
        individ.Center.Y.Should().Be(reslutPositionY);
    }
}