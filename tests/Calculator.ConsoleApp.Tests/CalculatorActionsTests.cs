using Calculator.StringCalculator;
using Moq;

namespace Calculator.ConsoleApp.Tests;

public class CalculatorActionsTests
{
    private readonly Mock<ICustomConsole> _consoleMock;
    private readonly Mock<ICalculatorFactory> _calculatorFactoryMock;
    private readonly Mock<ICalculator> _expressionCalculatorMock;
    private readonly CalculatorOptions _options;
    private readonly CalculatorActions _actions;

    public CalculatorActionsTests()
    {
        _consoleMock = new Mock<ICustomConsole>();
        _calculatorFactoryMock = new Mock<ICalculatorFactory>();
        _expressionCalculatorMock = new Mock<ICalculator>();
        _options = new CalculatorOptions();
        _actions = new CalculatorActions(_calculatorFactoryMock.Object, _options, _consoleMock.Object);
    }
    
    [Fact]
    public void PromptForSelection_WhenValidKeyPressed_ShouldReturnValidKey()
    {
        // Arrange
        _consoleMock.Setup(c => c.PromptKey("Select an option (Ctrl+C to quit): ")).Returns('a');

        // Act
        string result = _actions.PromptForSelection();

        // Assert
        Assert.Equal("a", result);
        _consoleMock.Verify(c => c.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
        _consoleMock.Verify(c => c.PromptKey(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void PromptForSelection_WhenInvalidKeyPressed_ShouldHandleInvalidKeyGracefully()
    {
        // Arrange
        _consoleMock.Setup(c => c.PromptKey("Select an option (Ctrl+C to quit): ")).Returns('!');

        // Act
        string result = _actions.PromptForSelection();

        // Assert
        Assert.Equal("!", result);
        _consoleMock.Verify(c => c.PromptKey(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void PerformOperation_WhenInputIsValid_ShouldCalculateResult()
    {
        // Arrange
        _consoleMock.SetupSequence(c => c.Prompt("> "))
            .Returns("1,2")
            .Returns("3,4")
            .Returns("done");
        _expressionCalculatorMock.Setup(c => c.Calculate("1,2\n3,4")).Returns("1+2+3+4 = 10");
        _calculatorFactoryMock.Setup(f => f.Create(Operations.Add)).Returns(_expressionCalculatorMock.Object);

        // Act
        _actions.PerformOperation(Operations.Add);

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Result: 1+2+3+4 = 10"), Times.Once);
    }

    [Fact]
    public void PerformOperation_WhenInvalidInput_ShouldSetInvalidInputToZero()
    {
        // Arrange
        _consoleMock.SetupSequence(c => c.Prompt("> "))
            .Returns("1,2")
            .Returns("invalid")
            .Returns("done");
        _expressionCalculatorMock.Setup(c => c.Calculate("1,2\ninvalid")).Returns("1+2+0 = 3");
        _calculatorFactoryMock.Setup(f => f.Create(Operations.Add)).Returns(_expressionCalculatorMock.Object);

        // Act
        _actions.PerformOperation(Operations.Add);

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Result: 1+2+0 = 3"), Times.Once);
    }
    
    [Fact]
    public void PromptForDelimiter_WhenInputIsValid_ShouldSetDelimiter()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Enter alternate delimiter: ")).Returns(";");

        // Act
        _actions.PromptForDelimiter();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Delimiters: [, ;]"), Times.Once);
        Assert.Equal([",", ";"], _options.DefaultDelimiters);
    }

    [Fact]
    public void PromptForDelimiter_WhenNoInput_ShouldHandleEmptyInput()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Enter alternate delimiter: ")).Returns("");

        // Act
        _actions.PromptForDelimiter();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Error: No delimiter provided."), Times.Once);
        Assert.Equal("\n", _options.DefaultDelimiters[1]); // No change
    }

    [Fact]
    public void PromptForDelimiter_WhenInputIsNumber_ShouldRejectNumberAsDelimiter()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Enter alternate delimiter: ")).Returns("1");

        // Act
        _actions.PromptForDelimiter();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Delimiters: [, 1]"), Times.Never);
        _consoleMock.Verify(c => c.WriteLine("Error: The delimiter cannot be a number."), Times.Once);
    }

    // Tests for PromptForAllowNegatives
    [Fact]
    public void PromptForAllowNegatives_WhenInputIsYes_ShouldEnableNegativeNumbers()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Allow negative numbers (y/n): ")).Returns("y");

        // Act
        _actions.PromptForAllowNegatives();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Allow negative numbers: Yes"), Times.Once);
        Assert.True(_options.AllowNegativeNumbers);
    }

    [Fact]
    public void PromptForAllowNegatives_WhenInputIsNo_ShouldEnableNegativeNumbers()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Allow negative numbers (y/n): ")).Returns("n");

        // Act
        _actions.PromptForAllowNegatives();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Allow negative numbers: No"), Times.Once);
        Assert.False(_options.AllowNegativeNumbers);
    }
    
    [Fact]
    public void PromptForMaxNumber_WhenInputIsValid_ShouldSetMaxNumber()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Enter the max number: ")).Returns("2000");

        // Act
        _actions.PromptForMaxNumber();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Max number: 2000"), Times.Once);
        Assert.Equal(2000, _options.MaxNumber);
    }

    [Fact]
    public void PromptForMaxNumber_ShouldHandleNonNumericInput()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Enter the max number: ")).Returns("XVIII");

        // Act
        _actions.PromptForMaxNumber();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Error: The max number is not valid."), Times.Once);
    }

    [Fact]
    public void PromptForMaxNumber_ShouldRejectNumbersLessThanOne()
    {
        // Arrange
        _consoleMock.Setup(c => c.Prompt("> Enter the max number: ")).Returns("0");

        // Act
        _actions.PromptForMaxNumber();

        // Assert
        _consoleMock.Verify(c => c.WriteLine("Max number: 0"), Times.Never);
        _consoleMock.Verify(c => c.WriteLine("Error: The max number cannot be less than 1."), Times.Once);
    }
}