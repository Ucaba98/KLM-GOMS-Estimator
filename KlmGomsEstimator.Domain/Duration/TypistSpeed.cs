namespace KlmGomsEstimator.Domain.Duration;

public readonly record struct TypistSpeed
{
    public readonly static TypistSpeed BestTypist = new("Best typist", 135, .08);
    public readonly static TypistSpeed GoodTypist = new("Good typist", 90, .12);
    public readonly static TypistSpeed AverageTypist = new("Average typist", 55, .20);
    public readonly static TypistSpeed AverageNonSecretaryTypist = new("Average non-secretary typist", 40, .28);
    public readonly static TypistSpeed WorstTypist = new("Worst typist", 9, 1.20);

    public string Description { get; }
    public int WordsPerMinute { get; }
    public double SecondsPerKeystroke { get; }

    private TypistSpeed(string description, int wordsPerMinute, double secondsPerKeystroke)
    {
        Description = description;
        WordsPerMinute = wordsPerMinute;
        SecondsPerKeystroke = secondsPerKeystroke;
    }

    public static implicit operator double(TypistSpeed typistSpeed) => typistSpeed.SecondsPerKeystroke;

    public static double operator *(TypistSpeed typistSpeed, double keystrokes) => typistSpeed.SecondsPerKeystroke * keystrokes;

    public override readonly string ToString() => $"{Description} - {WordsPerMinute}wpm";
}
