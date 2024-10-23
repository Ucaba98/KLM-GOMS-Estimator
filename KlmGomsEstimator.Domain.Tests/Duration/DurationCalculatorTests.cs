using FluentAssertions;
using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;
using NSubstitute;

namespace KlmGomsEstimator.Domain.Tests.Duration;

public class DurationCalculatorTests
{
    private readonly DurationCalculator _durationCalculatorSut;

    public DurationCalculatorTests()
    {
        _durationCalculatorSut = new DurationCalculator();
    }

    [Fact]
    public void CalculateDuration_Model_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var step1 = new Step("User presses button", new ButtonPressOperator());
        var step2 = new Step("User moves hands to keyboard", new HomingOperator());
        var instruction1 = new Instruction("Instruction 1");
        instruction1.AddStep(step1);
        var instruction2 = new Instruction("Instruction 2");
        instruction2.AddStep(step2);
        var model = new Model("Test Model");
        model.AddInstruction(instruction1);
        model.AddInstruction(instruction2);

        // Act
        var result = _durationCalculatorSut.CalculateDuration(model, typistSpeed);

        // Assert
        result.Should().Be(ButtonPressOperator.ButtonPressDuration + HomingOperator.HomingDuration);
    }

    [Fact]
    public void CalculateDuration_Instruction_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var step1 = new Step("User presses button", new ButtonPressOperator());
        var step2 = new Step("User presses button", new HomingOperator());
        var instruction = new Instruction("Instruction");
        instruction.AddStep(step1);
        instruction.AddStep(step2);

        // Act
        var result = _durationCalculatorSut.CalculateDuration(instruction, typistSpeed);

        // Assert
        result.Should().Be(ButtonPressOperator.ButtonPressDuration + HomingOperator.HomingDuration);
    }

    [Fact]
    public void CalculateDuration_Step_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var step = new Step("User presses button", new ButtonPressOperator());

        // Act
        var result = _durationCalculatorSut.CalculateDuration(step, typistSpeed);

        // Assert
        result.Should().Be(ButtonPressOperator.ButtonPressDuration);
    }

    [Fact]
    public void CalculateDuration_ButtonPressOperator_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var buttonPressOperator = new ButtonPressOperator();

        // Act
        var result = _durationCalculatorSut.CalculateDuration(buttonPressOperator, typistSpeed);

        // Assert
        result.Should().Be(ButtonPressOperator.ButtonPressDuration);
    }

    [Fact]
    public void CalculateDuration_HomingOperator_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var homingOperator = new HomingOperator();

        // Act
        var result = _durationCalculatorSut.CalculateDuration(homingOperator, typistSpeed);

        // Assert
        result.Should().Be(HomingOperator.HomingDuration);
    }

    [Fact]
    public void CalculateDuration_MentalPreparationOperator_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var mentalPreparationOperator = new MentalPreparationOperator();

        // Act
        var result = _durationCalculatorSut.CalculateDuration(mentalPreparationOperator, typistSpeed);

        // Assert
        result.Should().Be(MentalPreparationOperator.MentalPreparationDuration);
    }

    [Fact]
    public void CalculateDuration_PointingOperator_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var pointingOperator = new PointingOperator();

        // Act
        var result = _durationCalculatorSut.CalculateDuration(pointingOperator, typistSpeed);

        // Assert
        result.Should().Be(PointingOperator.PointingDuration);
    }

    [Fact]
    public void CalculateDuration_SystemResponseTimeOperator_ReturnsCorrectDuration()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var systemResponseTimeOperator = new SystemResponseTimeOperator();

        // Act
        var result = _durationCalculatorSut.CalculateDuration(systemResponseTimeOperator, typistSpeed);

        // Assert
        result.Should().Be(SystemResponseTimeOperator.SystemResponseTimeDuration);
    }

    [Theory]
    [MemberData(nameof(TypistSpeeds))]
    public void CalculateDuration_KeystrokeOperatorWithRegularComplexity_ReturnsCorrectDuration(TypistSpeed typistSpeed)
    {
        // Arrange
        var keystrokeOperator = new KeystrokeOperator(5, KeystrokeComplexity.Regular);

        // Act
        var result = _durationCalculatorSut.CalculateDuration(keystrokeOperator, typistSpeed);

        // Assert
        result.Should().Be(5 * typistSpeed);
    }

    [Theory]
    [MemberData(nameof(TypistSpeeds))]
    public void CalculateDuration_KeystrokeOperatorWithHighComplexity_ReturnsCorrectDuration(TypistSpeed typistSpeed)
    {
        // Arrange
        var keystrokeOperator = new KeystrokeOperator(5, KeystrokeComplexity.Complex);

        // Act
        var result = _durationCalculatorSut.CalculateDuration(keystrokeOperator, typistSpeed);

        // Assert
        result.Should().Be(5 * KeystrokeOperator.ComplexCodeTypingSpeed);
    }

    [Theory]
    [MemberData(nameof(TypistSpeeds))]
    public void CalculateDuration_KeystrokeOperatorWithRandomComplexity_ReturnsCorrectDuration(TypistSpeed typistSpeed)
    {
        // Arrange
        var keystrokeOperator = new KeystrokeOperator(5, KeystrokeComplexity.Random);

        // Act
        var result = _durationCalculatorSut.CalculateDuration(keystrokeOperator, typistSpeed);

        // Assert
        result.Should().Be(5 * KeystrokeOperator.RandomLetterTypingSpeed);
    }

    [Fact]
    public void CalculateDuration_UnsupportedOperator_ThrowsNotImplementedException()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var unsupportedOperator = Substitute.For<IOperator>();

        // Act
        Action act = () => _durationCalculatorSut.CalculateDuration(unsupportedOperator, typistSpeed);

        // Assert
        act.Should().Throw<NotImplementedException>();
    }

    public static IEnumerable<object[]> TypistSpeeds =>
    [
        [TypistSpeed.BestTypist],
        [TypistSpeed.GoodTypist],
        [TypistSpeed.AverageTypist],
        [TypistSpeed.AverageNonSecretaryTypist],
        [TypistSpeed.WorstTypist],
    ];
}
