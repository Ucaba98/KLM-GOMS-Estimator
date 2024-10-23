using FluentAssertions;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;

namespace KlmGomsEstimator.Domain.Tests.Instructions;

public class ModelTests
{
    [Fact]
    public void AddInstruction_AddsInstructionToList()
    {
        // Arrange
        var model = new Model("Test Model");
        var instruction = new Instruction("Test Instruction");

        model.Instructions.Add(instruction);

        // Act
        model.AddInstruction(instruction);

        // Assert
        model.Instructions.Should().Contain(instruction);
    }

    [Fact]
    public void RemoveInstruction_RemovesInstructionFromList()
    {
        // Arrange
        var model = new Model("Test Model");
        var instruction = new Instruction("Test Instruction");
        model.AddInstruction(instruction);

        // Act
        var result = model.RemoveInstruction(0);

        // Assert
        result.Should().BeTrue();
        model.Instructions.Should().NotContain(instruction);
    }

    [Fact]
    public void RemoveInstruction_WithInvalidIndex_ReturnsFalse()
    {
        // Arrange
        var model = new Model("Test Model");

        // Act
        var result = model.RemoveInstruction(0);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void MoveInstruction_MovesInstructionWithinList()
    {
        // Arrange
        var model = new Model("Test Model");
        var instruction1 = new Instruction("Instruction 1");
        var instruction2 = new Instruction("Instruction 2");
        model.AddInstruction(instruction1);
        model.AddInstruction(instruction2);

        // Act
        var result = model.MoveInstruction(0, 1);

        // Assert
        result.Should().BeTrue();
        model.Instructions[0].Should().Be(instruction2);
        model.Instructions[1].Should().Be(instruction1);
    }

    [Fact]
    public void MoveInstruction_WithInvalidIndex_ReturnsFalse()
    {
        // Arrange
        var model = new Model("Test Model");
        var instruction = new Instruction("Test Instruction");
        model.AddInstruction(instruction);

        // Act
        var result = model.MoveInstruction(0, 2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetKlmCode_ReturnsCorrectKlmCode()
    {
        // Arrange
        var model = new Model("Test Model");

        var instruction1 = new Instruction("Test Instruction");
        instruction1.AddStep(new Step("Test Step 1", new KeystrokeOperator(1, KeystrokeComplexity.Regular)));
        instruction1.AddStep(new Step("Test Step 2", new MentalPreparationOperator()));

        var instruction2 = new Instruction("Test Instruction");
        instruction1.AddStep(new Step("Test Step 1", new KeystrokeOperator(2, KeystrokeComplexity.Regular)));
        instruction1.AddStep(new Step("Test Step 2", new HomingOperator()));

        model.AddInstruction(instruction1);
        model.AddInstruction(instruction2);

        // Act
        var klmCode = model.GetKlmCode();

        // Assert
        klmCode.Should().Be("KM2KH");
    }
}
