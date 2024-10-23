using FluentAssertions;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;
using KlmGomsEstimator.Infrastructure.Dtos;

namespace KlmGomsEstimator.Infrastructure.Tests.Dtos;

public class ModelMappingExtensionsTests
{
    [Fact]
    public void ToJsonDto_ModelWithInstructions_ReturnsCorrectDto()
    {
        // Arrange
        var step1 = new Step("Step 1", new ButtonPressOperator());
        var step2 = new Step("Step 2", new HomingOperator());
        var instruction = new Instruction("Instruction 1");
        instruction.AddStep(step1);
        instruction.AddStep(step2);
        var model = new Model("Test Model");
        model.AddInstruction(instruction);

        // Act
        var result = model.ToJsonDto();

        // Assert
        result.Description.Should().Be("Test Model");
        result.Instructions.Should().HaveCount(1);
        result.Instructions[0].Description.Should().Be("Instruction 1");
        result.Instructions[0].Steps.Should().HaveCount(2);
        result.Instructions[0].Steps[0].Description.Should().Be("Step 1");
        result.Instructions[0].Steps[0].OperatorCode.Should().Be("B");
        result.Instructions[0].Steps[1].Description.Should().Be("Step 2");
        result.Instructions[0].Steps[1].OperatorCode.Should().Be("H");
    }

    [Theory]
    [InlineData(KeystrokeComplexity.Regular)]
    [InlineData(KeystrokeComplexity.Random)]
    [InlineData(KeystrokeComplexity.Complex)]
    public void ToJsonDto_ValidKeystrokeCode_ReturnsCorrectDto(KeystrokeComplexity complexity)
    {
        // Arrange
        var step = new Step("Step 1", new KeystrokeOperator(3, complexity));
        var instruction = new Instruction("Instruction 1");
        instruction.AddStep(step);
        var model = new Model("Test Model");
        model.AddInstruction(instruction);

        // Act
        var result = model.ToJsonDto();

        // Assert
        result.Description.Should().Be("Test Model");
        result.Instructions.Should().HaveCount(1);
        result.Instructions[0].Description.Should().Be("Instruction 1");
        result.Instructions[0].Steps.Should().HaveCount(1);
        result.Instructions[0].Steps[0].Description.Should().Be("Step 1");
        result.Instructions[0].Steps[0].OperatorCode.Should().Be($"3K{(int)complexity}");
    }

    [Fact]
    public void ToModel_ModelJsonDtoWithInstructions_ReturnsCorrectModel()
    {
        // Arrange
        var stepDto1 = new StepJsonDto("Step 1", "B");
        var stepDto2 = new StepJsonDto("Step 2", "H");
        var instructionDto = new InstructionJsonDto("Instruction 1", [stepDto1, stepDto2]);
        var modelDto = new ModelJsonDto("Test Model", [instructionDto]);

        // Act
        var result = modelDto.ToModel();

        // Assert
        result.Description.Should().Be("Test Model");
        result.Instructions.Should().HaveCount(1);
        result.Instructions[0].Description.Should().Be("Instruction 1");
        result.Instructions[0].Steps.Should().HaveCount(2);
        result.Instructions[0].Steps[0].Description.Should().Be("Step 1");
        result.Instructions[0].Steps[0].Operator.Should().BeOfType<ButtonPressOperator>();
        result.Instructions[0].Steps[1].Description.Should().Be("Step 2");
        result.Instructions[0].Steps[1].Operator.Should().BeOfType<HomingOperator>();
    }

    [Fact]
    public void ToModel_ValidKeystrokeCode_ReturnsCorrectOperator()
    {
        // Arrange
        var stepDto = new StepJsonDto("Step 1", "3K1");
        var instructionDto = new InstructionJsonDto("Instruction 1", new List<StepJsonDto> { stepDto });
        var modelDto = new ModelJsonDto("Test Model", new List<InstructionJsonDto> { instructionDto });

        // Act
        var result = modelDto.ToModel();

        // Assert
        var step = result.Instructions[0].Steps[0];
        step.Operator.Should().BeOfType<KeystrokeOperator>();
        var keystrokeOperator = (KeystrokeOperator)step.Operator;
        keystrokeOperator.Keystrokes.Should().Be(3);
        keystrokeOperator.Complexity.Should().Be(KeystrokeComplexity.Random);
    }

    [Fact]
    public void ToModel_InvalidKeystrokeCode_ThrowsException()
    {
        // Arrange
        var stepDto = new StepJsonDto("Step 1", "InvalidCode");
        var instructionDto = new InstructionJsonDto("Instruction 1", new List<StepJsonDto> { stepDto });
        var modelDto = new ModelJsonDto("Test Model", [instructionDto]);

        // Act
        Action act = () => modelDto.ToModel();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Unknown operator type.");
    }
}
