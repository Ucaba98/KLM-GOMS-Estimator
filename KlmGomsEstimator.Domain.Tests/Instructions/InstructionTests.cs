using FluentAssertions;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;

namespace KlmGomsEstimator.Domain.Tests.Instructions;

public class InstructionTests
{
    [Fact]
    public void AddStep_AddsStepToList()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step = new Step("Step 1", new HomingOperator());

        // Act
        instruction.AddStep(step);

        // Assert
        instruction.Steps.Should().Contain(step);
    }

    [Fact]
    public void RemoveStep_RemovesStepFromList()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step = new Step("Step 1", new HomingOperator());
        instruction.AddStep(step);

        // Act
        var result = instruction.RemoveStep(0);

        // Assert
        result.Should().BeTrue();
        instruction.Steps.Should().NotContain(step);
    }

    [Fact]
    public void RemoveStep_WithInvalidIndex_ReturnsFalse()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");

        // Act
        var result = instruction.RemoveStep(0);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void MoveStep_MovesStepWithinList()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step1 = new Step("Step 1", new HomingOperator());
        var step2 = new Step("Step 2", new MentalPreparationOperator());

        instruction.AddStep(step1);
        instruction.AddStep(step2);

        // Act
        var result = instruction.MoveStep(0, 1);

        // Assert
        result.Should().BeTrue();
        instruction.Steps[0].Should().Be(step2);
        instruction.Steps[1].Should().Be(step1);
    }

    [Fact]
    public void MoveStep_WithInvalidIndex_ReturnsFalse()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step = new Step("Step 1", new HomingOperator());
        instruction.AddStep(step);

        // Act
        var result = instruction.MoveStep(0, 2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetKlmCode_ReturnsCorrectKlmCode()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step = new Step("Step 1", new KeystrokeOperator(2, KeystrokeComplexity.Regular));
        instruction.AddStep(step);

        // Act
        var klmCode = instruction.GetKlmCode();

        // Assert
        klmCode.Should().Be("2K");
    }
}
