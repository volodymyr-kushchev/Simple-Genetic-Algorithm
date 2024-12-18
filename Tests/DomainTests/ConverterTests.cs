using System.Drawing;
using Domain.Services;
using FluentAssertions;

namespace Tests.DomainTests;

public class ConverterTests
{
    [Theory]
    [ClassData(typeof(ConverterChromosomToStringTestData))]
    public void Convert_Chromosome_ReturnString(bool[] chromosome, string expected)
    {
        // act
        var result = Converter.ChromosomeToString(chromosome);
        
        //assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Convert_ConvertByteToBoolArray()
    {
        // arrange
        byte byteValue = 2;
        
        // act
        var result = Converter.ConvertByteToBoolArray(byteValue);
        
        // assert
        result.Length.Should().Be(8);
        result.Should().BeEquivalentTo([false, false, false, false, false, false, true, false]);
    }

    [Fact]
    public void Convert_ConvertBoolArrayToByte()
    {
        // arrange
        var boolArray = new bool[] { false, false, false, false, false, false, true, false };
        
        // act
        var result = Converter.ConvertBoolArrayToByte(boolArray);
        
        // assert
        result.Should().Be(2);
    }

    [Fact]
    public void Convert_FromColorToBool()
    {
        // arrange
        var color = Color.White;
        
        // act
        var result = Converter.FromColorToBool(color);
        
        // assert
        result.Should().BeEquivalentTo([true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true]);
    }

    [Fact]
    public void Convert_FromBoolToColor()
    {
        // arrange
        var boolArray = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }; 
        
        // act
        Color result = Converter.FromBoolToColor(boolArray);
        
        // assert
        result.ToArgb().Should().Be(Color.White.ToArgb());
    }
}