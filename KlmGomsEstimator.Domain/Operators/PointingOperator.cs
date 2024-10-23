namespace KlmGomsEstimator.Domain.Operators;

public readonly record struct PointingOperator : IOperator
{
    public const double PointingDuration = 1.1;

    public string Symbol => "P";
}
