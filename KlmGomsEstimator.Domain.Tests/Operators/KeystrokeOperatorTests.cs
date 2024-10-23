using FluentAssertions;
using KlmGomsEstimator.Domain.Operators;

namespace KlmGomsEstimator.Domain.Tests.Operators;

public class KeystrokeOperatorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void KeystrokeOperator_WhenFewerThanOneKeystroke_ThrowsArgumentException(int keystrokes)
    {
        // Act
        Action act = () => _ = new KeystrokeOperator(keystrokes, KeystrokeComplexity.Regular);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(1, "K")]
    [InlineData(5, "5K")]
    public void OperatorSymbol_ReturnsCorrectSymbol(int keystrokes, string expectedSymbol)
    {
        // Act
        var keystrokeOperator = new KeystrokeOperator(keystrokes, KeystrokeComplexity.Regular);

        // Assert
        keystrokeOperator.Symbol.Should().Be(expectedSymbol);
    }
}
