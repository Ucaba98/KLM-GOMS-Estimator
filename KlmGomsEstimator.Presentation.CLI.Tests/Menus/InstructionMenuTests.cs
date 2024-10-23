using FluentAssertions;
using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;
using KlmGomsEstimator.Presentation.CLI.Menus;
using KlmGomsEstimator.Presentation.CLI.Terminal;
using NSubstitute;

namespace KlmGomsEstimator.Presentation.CLI.Tests.Menus;

public class InstructionMenuTests
{
    private readonly IConsole _consoleMock;
    private readonly IStepMenu _stepMenuMock;
    private readonly IModelBeautifier _modelBeautifierMock;

    private readonly InstructionMenu _instructionMenuSut;

    public InstructionMenuTests()
    {
        _consoleMock = Substitute.For<IConsole>();
        _stepMenuMock = Substitute.For<IStepMenu>();
        _modelBeautifierMock = Substitute.For<IModelBeautifier>();

        _instructionMenuSut = new InstructionMenu(_consoleMock, _stepMenuMock, _modelBeautifierMock);
    }

    [Fact]
    public void ReadInstruction_ReturnsInstructionWithCorrectDescriptionAndSteps()
    {
        // Arrange
        var instructionDescription = "Test Instruction";
        var stepDescription = "Test Step";
        var typistSpeed = TypistSpeed.AverageTypist;
        var step = new Step(stepDescription, new HomingOperator());

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(instructionDescription);
        _consoleMock.ReadLine().Returns("q");
        _stepMenuMock.ReadStep().Returns(step);

        // Act
        var result = _instructionMenuSut.ReadInstruction();

        // Assert
        result.Description.Should().Be(instructionDescription);
        result.Steps.Should().ContainSingle(s => s.Description == stepDescription && s.Operator is HomingOperator);
    }

    [Fact]
    public void ReadInstruction_InvalidStepDescription_ThrowsException()
    {
        // Arrange
        var instructionDescription = "Test Instruction";

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(instructionDescription);
        _consoleMock.ReadLine().Returns("a");
        _stepMenuMock
            .ReadStep()
            .Returns(x => { throw new InvalidOperationException("Invalid step description"); });

        // Act
        Action act = () => _instructionMenuSut.ReadInstruction();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Invalid step description");
    }

    [Fact]
    public void ModifyInstruction_AddStep()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step = new Step("Test Step", new HomingOperator());

        _consoleMock.ReadLine().Returns("a", "q");
        _stepMenuMock.ReadStep().Returns(step);

        // Act
        _instructionMenuSut.ModifyInstruction(instruction);

        // Assert
        instruction.Steps.Should().ContainSingle(s => s.Description == "Test Step" && s.Operator is HomingOperator);
    }

    [Fact]
    public void ModifyInstruction_RemoveStep()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step = new Step("Test Step", new HomingOperator());
        instruction.AddStep(step);

        _consoleMock.ReadLine().Returns("r", "q");
        _consoleMock.ReadIndex(Arg.Any<string>(), 1, 1).Returns(0);

        // Act
        _instructionMenuSut.ModifyInstruction(instruction);

        // Assert
        instruction.Steps.Should().BeEmpty();
    }

    [Fact]
    public void ModifyInstruction_MoveStep()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var step1 = new Step("Step 1", new HomingOperator());
        var step2 = new Step("Step 2", new HomingOperator());
        instruction.AddStep(step1);
        instruction.AddStep(step2);

        _consoleMock.ReadLine().Returns("m", "q");
        _consoleMock
            .ReadIndex(Arg.Any<string>(), 1, 2)
            .Returns(0, 1);

        // Act
        _instructionMenuSut.ModifyInstruction(instruction);

        // Assert
        instruction.Steps[0].Should().Be(step2);
        instruction.Steps[1].Should().Be(step1);
    }

    [Fact]
    public void ModifyInstruction_ChangeDescription()
    {
        // Arrange
        var instruction = new Instruction("Test Instruction");
        var newDescription = "New Description";

        _consoleMock.ReadLine().Returns("d", "q");
        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(newDescription);

        // Act
        _instructionMenuSut.ModifyInstruction(instruction);

        // Assert
        instruction.Description.Should().Be(newDescription);
    }
}
