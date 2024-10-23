using FluentAssertions;
using KlmGomsEstimator.Presentation.CLI.Menus.Options;
using KlmGomsEstimator.Presentation.CLI.Terminal;
using NSubstitute;

namespace KlmGomsEstimator.Presentation.CLI.Tests.Menus.Options;

public class OptionsMenuTests
{
    private readonly IConsole _consoleMock;

    public OptionsMenuTests()
    {
        _consoleMock = Substitute.For<IConsole>();
    }

    [Fact]
    public void ReadOption_ReturnsCorrectChoice()
    {
        // Arrange
        var optionDescription = "Test Option";
        var option = new TerminalOption(optionDescription, 't');

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(optionDescription);
        _consoleMock.ReadLine().Returns("t");

        OptionsMenu menu = new("Test Menu", _consoleMock) { option };

        // Act
        var result = menu.ReadOption();

        // Assert
        result.Text.Should().Be(optionDescription);
    }

    [Fact]
    public void ReadOption_SkipsInvalidChoices()
    {
        // Arrange
        var optionDescription = "Test Option";
        var option = new TerminalOption(optionDescription, 't');

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(optionDescription);
        _consoleMock.ReadLine().Returns("a", "q");

        OptionsMenu menu = new("Test Menu", _consoleMock) { option, TerminalOption.Quit };

        // Act
        var choice = menu.ReadOption();

        // Assert
        choice.Should().Be(TerminalOption.Quit);
    }

    [Fact]
    public void ReadOption_WhenNoOptionsExist_ThrowsException()
    {
        // Arrange
        OptionsMenu menu = new("Test Menu", _consoleMock);

        // Act
        Action act = () => menu.ReadOption();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("No options available.");
    }
}
