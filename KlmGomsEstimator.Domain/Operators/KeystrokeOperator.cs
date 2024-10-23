namespace KlmGomsEstimator.Domain.Operators;

public readonly record struct KeystrokeOperator(int Keystrokes, KeystrokeComplexity Complexity) : IOperator
{
    public const double RandomLetterTypingSpeed = 0.5;
    public const double ComplexCodeTypingSpeed = 0.75;

    public string Symbol => _keystrokes == 1 ? "K" : $"{_keystrokes}K";

    private readonly int _keystrokes =
        Keystrokes >= 1
        ? Keystrokes
        : throw new ArgumentException("There needs to be at least one keystroke per Keystroke operator", nameof(Keystrokes));
}

public enum KeystrokeComplexity
{
    Regular,
    Random,
    Complex
}
