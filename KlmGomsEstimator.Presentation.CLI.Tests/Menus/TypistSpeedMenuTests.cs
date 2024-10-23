using FluentAssertions;
using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Presentation.CLI.Menus;
using KlmGomsEstimator.Presentation.CLI.Terminal;
using NSubstitute;

namespace KlmGomsEstimator.Presentation.CLI.Tests.Menus;

public class TypistSpeedMenuTests
{
    private readonly IConsole _consoleMock;
    private readonly TypistSpeedMenu _typistSpeedMenuSut;

    public TypistSpeedMenuTests()
    {
        _consoleMock = Substitute.For<IConsole>();
        _typistSpeedMenuSut = new TypistSpeedMenu(_consoleMock);
    }

    [Fact]
    public void ChooseTypistSpeed_ReturnsCorrectTypistSpeed()
    {
        // Arrange
        var expectedTypistSpeed = TypistSpeed.AverageTypist;
        _consoleMock.ReadIndex(Arg.Any<string>(), 1, 5).Returns(2);

        // Act
        var result = _typistSpeedMenuSut.ChooseTypistSpeed();

        // Assert
        result.Should().Be(expectedTypistSpeed);
    }

    [Fact]
    public void ChooseTypistSpeed_DisplaysTypistSpeedsCorrectly()
    {
        // Arrange
        _consoleMock.ReadIndex(Arg.Any<string>(), 1, 5).Returns(2);

        // Act
        _typistSpeedMenuSut.ChooseTypistSpeed();

        // Assert
        _consoleMock.Received().WriteLine("Typist speeds:");
        _consoleMock.Received().WriteLine("1. Best typist - 135wpm");
        _consoleMock.Received().WriteLine("2. Good typist - 90wpm");
        _consoleMock.Received().WriteLine("3. Average typist - 55wpm");
        _consoleMock.Received().WriteLine("4. Average non-secretary typist - 40wpm");
        _consoleMock.Received().WriteLine("5. Worst typist - 9wpm");
    }
}
