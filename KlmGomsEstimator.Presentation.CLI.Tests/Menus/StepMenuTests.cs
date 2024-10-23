using FluentAssertions;
using KlmGomsEstimator.Domain.Operators;
using KlmGomsEstimator.Presentation.CLI.Menus;
using KlmGomsEstimator.Presentation.CLI.Terminal;
using NSubstitute;

namespace KlmGomsEstimator.Presentation.CLI.Tests.Menus;

public class StepMenuTests
{
    private readonly IConsole _consoleMock;

    private readonly StepMenu _stepMenuSut;

    public StepMenuTests()
    {
        _consoleMock = Substitute.For<IConsole>();

        _stepMenuSut = new StepMenu(_consoleMock);
    }

    [Fact]
    public void ReadStep_ReturnsStepWithCorrectDescriptionAndHomingOperator()
    {
        // Arrange
        var stepDescription = "Test Step";
        var expectedOperator = new HomingOperator();

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(stepDescription);
        _consoleMock.ReadLine().Returns("h");

        // Act
        var result = _stepMenuSut.ReadStep();

        // Assert
        result.Description.Should().Be(stepDescription);
        result.Operator.Should().Be(expectedOperator);
    }

    [Fact]
    public void ReadStep_ReturnsStepWithCorrectDescriptionAndKeystrokeOperator()
    {
        // Arrange
        var stepDescription = "Test Step";

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(stepDescription);
        _consoleMock.ReadLine().Returns("k");

        // Act
        var result = _stepMenuSut.ReadStep();

        // Assert
        result.Description.Should().Be(stepDescription);
        result.Operator.Should().BeOfType<KeystrokeOperator>();
        ((KeystrokeOperator)result.Operator).Keystrokes.Should().Be(1);
        ((KeystrokeOperator)result.Operator).Complexity.Should().Be(KeystrokeComplexity.Complex);
    }

    [Fact]
    public void ReadStep_ReturnsStepWithCorrectDescriptionAndKeystrokeOperatorWithNumberOfKeystrokes()
    {
        // Arrange
        var stepDescription = "Test Step";

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(stepDescription);
        _consoleMock.ReadLine().Returns("3k");
        _consoleMock.ReadIndex(Arg.Any<string>(), 1, 3).Returns(2);

        // Act
        var result = _stepMenuSut.ReadStep();

        // Assert
        result.Description.Should().Be(stepDescription);
        result.Operator.Should().BeOfType<KeystrokeOperator>();
        ((KeystrokeOperator)result.Operator).Keystrokes.Should().Be(3);
        ((KeystrokeOperator)result.Operator).Complexity.Should().Be(KeystrokeComplexity.Complex);
    }

    [Fact]
    public void ReadStep_InvalidOperatorInput_ShowsErrorMessageAndReturnsCorrectOperator()
    {
        // Arrange
        var stepDescription = "Test Step";

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(stepDescription);
        _consoleMock.ReadLine().Returns("invalid", "k");

        // Act
        var result = _stepMenuSut.ReadStep();

        // Assert
        _consoleMock.Received().WriteLine("Invalid operator. Please try again.");
        result.Description.Should().Be(stepDescription);
        result.Operator.Should().BeOfType<KeystrokeOperator>();
        ((KeystrokeOperator)result.Operator).Keystrokes.Should().Be(1);
        ((KeystrokeOperator)result.Operator).Complexity.Should().Be(KeystrokeComplexity.Complex);
    }
}
