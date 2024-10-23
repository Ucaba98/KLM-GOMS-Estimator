namespace KlmGomsEstimator.Domain.Operators;

public readonly record struct SystemResponseTimeOperator : IOperator
{
    public const double SystemResponseTimeDuration = 1;

    public string Symbol => "R";
}
