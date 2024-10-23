using FluentAssertions;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Presentation.CLI.Menus;
using KlmGomsEstimator.Presentation.CLI.Terminal;
using NSubstitute;

namespace KlmGomsEstimator.Presentation.CLI.Tests.Menus;

public class ModelMenuTests
{
    private readonly IConsole _consoleMock;
    private readonly IInstructionMenu _instructionMenuMock;
    private readonly ModelMenu _modelMenuSut;

    public ModelMenuTests()
    {
        _consoleMock = Substitute.For<IConsole>();
        _instructionMenuMock = Substitute.For<IInstructionMenu>();
        _modelMenuSut = new ModelMenu(_consoleMock, _instructionMenuMock);
    }

    [Fact]
    public void ReadModel_ReturnsModelWithCorrectDescription()
    {
        // Arrange
        var description = "Test Model";
        var instruction = new Instruction("Test Instruction");

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(description);
        _instructionMenuMock.ReadInstruction().Returns(instruction);
        _consoleMock.ReadLine().Returns("q");

        // Act
        var result = _modelMenuSut.ReadModel();

        // Assert
        result.Description.Should().Be(description);
        result.Instructions.Should().ContainSingle(i => i.Description == "Test Instruction");
    }

    [Fact]
    public void ModifyModel_AddInstruction()
    {
        // Arrange
        var model = new Model("Test Model");
        var instruction = new Instruction("Test Instruction");

        _consoleMock.ReadLine().Returns("a", "q");
        _instructionMenuMock.ReadInstruction().Returns(instruction);

        // Act
        _modelMenuSut.ModifyMenu(model);

        // Assert
        model.Instructions.Should().ContainSingle(i => i.Description == "Test Instruction");
    }

    [Fact]
    public void ModifyModel_RemoveInstruction()
    {
        // Arrange
        var model = new Model("Test Model");
        var instruction = new Instruction("Test Instruction");
        model.AddInstruction(instruction);

        _consoleMock.ReadLine().Returns("r", "q");
        _consoleMock.ReadIndex(Arg.Any<string>(), 1, 1).Returns(0);

        // Act
        _modelMenuSut.ModifyMenu(model);

        // Assert
        model.Instructions.Should().BeEmpty();
    }

    [Fact]
    public void ModifyModel_MoveInstruction()
    {
        // Arrange
        var model = new Model("Test Model");
        var instruction1 = new Instruction("Instruction 1");
        var instruction2 = new Instruction("Instruction 2");
        model.AddInstruction(instruction1);
        model.AddInstruction(instruction2);

        _consoleMock.ReadLine().Returns("m", "q");
        _consoleMock.ReadIndex(Arg.Any<string>(), 1, 2).Returns(0, 1);

        // Act
        _modelMenuSut.ModifyMenu(model);

        // Assert
        model.Instructions[0].Should().Be(instruction2);
        model.Instructions[1].Should().Be(instruction1);
    }

    [Fact]
    public void ModifyModel_ChangeDescription()
    {
        // Arrange
        var model = new Model("Test Model");
        var newDescription = "New Description";

        _consoleMock.ReadLine().Returns("d", "q");
        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(newDescription);

        // Act
        _modelMenuSut.ModifyMenu(model);

        // Assert
        model.Description.Should().Be(newDescription);
    }
}
