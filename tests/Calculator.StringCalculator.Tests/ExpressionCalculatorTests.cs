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
        Assert.Equal("1+50 = 51", fixture.AdditionCalculator.Calculate("1,50"));
        Assert.Equal("1-500-10 = -509", fixture.SubtractionCalculator.Calculate("1,500,10"));
        Assert.Equal("500/2/2/2/2 = 31.2500", fixture.DivisionCalculator.Calculate("500,2,2,2,2"));
    }
    
    [Fact]
    public void Calculate_WhenNegativeNumbersAllowed_ShouldReturnCorrectResult()
    {
        var calc = new ExpressionCalculator(new Multiplication(),
            new CalculatorOptions { AllowNegativeNumbers = true });
        Assert.Equal("4*-3*2*1 = -24", calc.Calculate("4,-3,2,1"));
    }

    [Fact]
    public void Calculate_WhenNegativeNumbersNotAllowed_ShouldThrowFormatException()
    {
        var calc = new ExpressionCalculator(new Multiplication(),
            new CalculatorOptions { AllowNegativeNumbers = false });
        var e = Assert.Throws<FormatException>(() => calc.Calculate("4,-3,2,-1"));
        Assert.Equal("Negative numbers are not allowed: -3,-1", e.Message);
    }

    [Fact]
    public void Calculate_WhenNumberIsGreaterThanMax_ShouldBeIgnored()
    {
        var calc = new ExpressionCalculator(new Division(),
            new CalculatorOptions { MaxNumber = 500 });
        
        Assert.Equal("0/2/5/10 = 0.0000", calc.Calculate("1000,2,5,10"));
        Assert.Equal("2+0+6 = 8", fixture.AdditionCalculator.Calculate("2,1001,6"));
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
    public void Calculate_WithAlternateDelimiter_ShouldReturnCorrectResult()
    {
        var calc = new ExpressionCalculator(new Subtraction(),
            new CalculatorOptions { DefaultDelimiters = [',', '!'] });
        
        Assert.Equal("4-3-2 = -1", calc.Calculate("4,3!2"));
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
     public void AlternateDelimiter_WhenSetToDigit_ShouldThrowArgumentException()
     {
         Assert.Throws<ArgumentException>(() =>
             new ExpressionCalculator(new Addition(), new CalculatorOptions { DefaultDelimiters = [',', '0'] }));
     }
     
     [Fact]
     public void MaxNumber_WhenSetToLessThanOne_ShouldThrowArgumentException()
     {
         Assert.Throws<ArgumentException>(() =>
             new ExpressionCalculator(new Addition(), new CalculatorOptions { MaxNumber = 0 }));
     }
}