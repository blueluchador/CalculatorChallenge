namespace Calculator.StringCalculator.Tests;

public class ExpressionCalculatorTests(CalculatorFixture fixture) : IClassFixture<CalculatorFixture>
{
    [Fact]
    public void Calculate_SingleNumber_ShouldReturnTheNumber()
    {
        Assert.Equal("20 = 20", fixture.AdditionCalculator.Calculate("20"));
        Assert.Equal("20 = 20", fixture.SubtractionCalculator.Calculate("20"));
        Assert.Equal("20 = 20", fixture.MultiplicationCalculator.Calculate("20"));
        Assert.Equal("20 = 20.0000", fixture.DivisionCalculator.Calculate("20"));
    }
    
    [Fact]
    public void Calculate_Numbers_ShouldReturnCorrectResult()
    {
        Assert.Equal("1+500 = 501", fixture.AdditionCalculator.Calculate("1,500"));
        Assert.Equal("1-500 = -499", fixture.SubtractionCalculator.Calculate("1,500"));
        Assert.Equal("4*-3 = -12", fixture.MultiplicationCalculator.Calculate("4,-3"));
        Assert.Equal("1/500 = 0.0020", fixture.DivisionCalculator.Calculate("1,500"));
    }

    [Fact]
    public void Calculate_WithInvalidNumbers_ShouldBeIgnored()
    {
        Assert.Equal("5+0 = 5", fixture.AdditionCalculator.Calculate("5,tytyt"));
        Assert.Equal("5-0 = 5", fixture.SubtractionCalculator.Calculate("5,tytyt"));
        Assert.Equal("5*0 = 0", fixture.MultiplicationCalculator.Calculate("5,tytyt"));
        Assert.Equal("0/5 = 0.0000", fixture.DivisionCalculator.Calculate("tytyt,5"));
    }
    
    [Fact]
    public void Calculate_WhenInputIsBlank_ShouldReturnZero()
    {
        Assert.Equal("0 = 0", fixture.AdditionCalculator.Calculate(""));
        Assert.Equal("0 = 0", fixture.SubtractionCalculator.Calculate(""));
        Assert.Equal("0 = 0", fixture.MultiplicationCalculator.Calculate(""));
        Assert.Equal("0 = 0.0000", fixture.DivisionCalculator.Calculate(""));
    }

    [Fact]
    public void Calculate_DivideNumberByZero_ShouldThrowDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => fixture.DivisionCalculator.Calculate("10,0"));
    }

    [Fact]
    public void Calculate_WhenInputIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => fixture.AdditionCalculator.Calculate(null));
    }

    [Fact]
    public void Calculate_WhenInputExceedsTheMaximum_ShouldThrowFormatException()
    {
        Assert.Throws<FormatException>(() => fixture.AdditionCalculator.Calculate("1,2,3"));
    }
}