using System.Drawing;
using Domain.Models;
using FluentAssertions;

namespace Tests.DomainTests;

public class IndividTests
{
    [Fact]
    public void Test1()
    {
        var individ = new Individ(new Point(100, 300), Color.Aqua);
        
        individ.Update();

        individ.IsChecked.Should().Be(false);
    }
}