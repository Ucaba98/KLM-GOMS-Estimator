using FluentAssertions;
using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;
using NSubstitute;
using System.Text;

namespace KlmGomsEstimator.Domain.Tests.Instructions;

public class ModelBeautifierTests
{
    private readonly IDurationCalculator _durationCalculatorMock;
    private readonly ModelBeautifier _modelBeautifierSut;

    public ModelBeautifierTests()
    {
        _durationCalculatorMock = Substitute.For<IDurationCalculator>();
        _modelBeautifierSut = new ModelBeautifier(_durationCalculatorMock);
    }

    [Fact]
    public void Constructor_InitializesModelBeautifier()
    {
        // Assert
        _modelBeautifierSut.Should().NotBeNull();
    }

    [Fact]
    public void BeautifyModel_ReturnsCorrectFormattedString()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var step1 = new Step("User moves hands to mouse", new HomingOperator());
        var step2 = new Step("User goes to form", new PointingOperator());
        var step3 = new Step("User moves hands to keyboard", new HomingOperator());
        var step4 = new Step("User presses button", new ButtonPressOperator());
        var instruction1 = new Instruction("First instruction description");
        instruction1.AddStep(step1);
        instruction1.AddStep(step2);
        var instruction2 = new Instruction("Second instruction description");
        instruction2.AddStep(step3);
        instruction2.AddStep(step4);
        var model = new Model("Test Model");
        model.AddInstruction(instruction1);
        model.AddInstruction(instruction2);

        _durationCalculatorMock.CalculateDuration(model, typistSpeed).Returns(2.0);
        _durationCalculatorMock.CalculateDuration(instruction1, typistSpeed).Returns(1.5);
        _durationCalculatorMock.CalculateDuration(instruction2, typistSpeed).Returns(0.5);
        _durationCalculatorMock.CalculateDuration(step1, typistSpeed).Returns(0.4);
        _durationCalculatorMock.CalculateDuration(step2, typistSpeed).Returns(1.1);
        _durationCalculatorMock.CalculateDuration(step3, typistSpeed).Returns(0.4);
        _durationCalculatorMock.CalculateDuration(step4, typistSpeed).Returns(0.1);

        // Act
        var result = _modelBeautifierSut.BeautifyModel(model, typistSpeed);

        // Assert
        var expectedOutput = new StringBuilder()
            .AppendLine("Model: Test Model")
            .AppendLine($"Typist Speed: {typistSpeed}")
            .AppendLine("KLM-GOMS: HPHB")
            .AppendLine("Duration: 2s")
            .AppendLine("\tInstruction 1:")
            .AppendLine("\tFirst instruction description")
            .AppendLine("\tKLM-GOMS: HP")
            .AppendLine("\tDuration: 1.5s")
            .AppendLine("\tSteps:")
            .AppendLine("\t\tStep 1:")
            .AppendLine("\t\tUser moves hands to mouse      H     = 0.4s")
            .AppendLine("\t\tStep 2:")
            .AppendLine("\t\tUser goes to form              P     = 1.1s")
            .AppendLine("\tInstruction 2:")
            .AppendLine("\tSecond instruction description")
            .AppendLine("\tKLM-GOMS: HB")
            .AppendLine("\tDuration: 0.5s")
            .AppendLine("\tSteps:")
            .AppendLine("\t\tStep 1:")
            .AppendLine("\t\tUser moves hands to keyboard   H     = 0.4s")
            .AppendLine("\t\tStep 2:")
            .AppendLine("\t\tUser presses button            B     = 0.1s")
            .ToString();

        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void BeautifyInstruction_ReturnsCorrectFormattedString()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var step1 = new Step("User moves hands to mouse", new HomingOperator());
        var step2 = new Step("User goes to form", new PointingOperator());
        var instruction = new Instruction("Instruction 1");
        instruction.AddStep(step1);
        instruction.AddStep(step2);

        _durationCalculatorMock.CalculateDuration(instruction, typistSpeed).Returns(1.5);
        _durationCalculatorMock.CalculateDuration(step1, typistSpeed).Returns(0.4);
        _durationCalculatorMock.CalculateDuration(step2, typistSpeed).Returns(1.1);

        // Act
        var result = _modelBeautifierSut.BeautifyInstruction(instruction, typistSpeed);

        // Assert
        var expectedOutput = new StringBuilder()
            .AppendLine("Instruction 1")
            .AppendLine("KLM-GOMS: HP")
            .AppendLine("Duration: 1.5s")
            .AppendLine("Steps:")
            .AppendLine("\tStep 1:")
            .AppendLine("\tUser moves hands to mouse      H     = 0.4s")
            .AppendLine("\tStep 2:")
            .AppendLine("\tUser goes to form              P     = 1.1s")
            .ToString();

        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void BeautifyStep_ReturnsCorrectFormattedString()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var step = new Step("User moves hands to mouse", new HomingOperator());

        _durationCalculatorMock.CalculateDuration(step, typistSpeed).Returns(0.4);

        // Act
        var result = _modelBeautifierSut.BeautifyStep(step, typistSpeed);

        // Assert
        var expectedOutput = "User moves hands to mouse      H     = 0.4s";

        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void BeautifyModel_WithNoTypistSpeed_ReturnsCorrectFormattedString()
    {
        // Arrange
        var step1 = new Step("User moves hands to mouse", new HomingOperator());
        var step2 = new Step("User goes to form", new PointingOperator());
        var step3 = new Step("User moves hands to keyboard", new HomingOperator());
        var step4 = new Step("User presses button", new ButtonPressOperator());
        var instruction1 = new Instruction("First instruction description");
        instruction1.AddStep(step1);
        instruction1.AddStep(step2);
        var instruction2 = new Instruction("Second instruction description");
        instruction2.AddStep(step3);
        instruction2.AddStep(step4);
        var model = new Model("Test Model");
        model.AddInstruction(instruction1);
        model.AddInstruction(instruction2);

        // Act
        var result = _modelBeautifierSut.BeautifyModel(model);

        // Assert
        var expectedOutput = new StringBuilder()
            .AppendLine("Model: Test Model")
            .AppendLine("KLM-GOMS: HPHB")
            .AppendLine("\tInstruction 1:")
            .AppendLine("\tFirst instruction description")
            .AppendLine("\tKLM-GOMS: HP")
            .AppendLine("\tSteps:")
            .AppendLine("\t\tStep 1:")
            .AppendLine("\t\tUser moves hands to mouse      H")
            .AppendLine("\t\tStep 2:")
            .AppendLine("\t\tUser goes to form              P")
            .AppendLine("\tInstruction 2:")
            .AppendLine("\tSecond instruction description")
            .AppendLine("\tKLM-GOMS: HB")
            .AppendLine("\tSteps:")
            .AppendLine("\t\tStep 1:")
            .AppendLine("\t\tUser moves hands to keyboard   H")
            .AppendLine("\t\tStep 2:")
            .AppendLine("\t\tUser presses button            B")
            .ToString();

        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void BeautifyInstruction_WithNoTypistSpeed_ReturnsCorrectFormattedString()
    {
        // Arrange
        var step1 = new Step("User moves hands to mouse", new HomingOperator());
        var step2 = new Step("User goes to form", new PointingOperator());
        var instruction = new Instruction("Instruction 1");
        instruction.AddStep(step1);
        instruction.AddStep(step2);

        // Act
        var result = _modelBeautifierSut.BeautifyInstruction(instruction);

        // Assert
        var expectedOutput = new StringBuilder()
            .AppendLine("Instruction 1")
            .AppendLine("KLM-GOMS: HP")
            .AppendLine("Steps:")
            .AppendLine("\tStep 1:")
            .AppendLine("\tUser moves hands to mouse      H")
            .AppendLine("\tStep 2:")
            .AppendLine("\tUser goes to form              P")
            .ToString();

        result.Should().Be(expectedOutput);
    }

    [Fact]
    public void BeautifyStep_WithNoTypistSpeed_ReturnsCorrectFormattedString()
    {
        // Arrange
        var step = new Step("User moves hands to mouse", new HomingOperator());

        // Act
        var result = _modelBeautifierSut.BeautifyStep(step);

        // Assert
        var expectedOutput = "User moves hands to mouse      H";

        result.Should().Be(expectedOutput);
    }
}
