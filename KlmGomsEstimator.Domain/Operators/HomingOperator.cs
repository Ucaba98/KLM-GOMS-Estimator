namespace KlmGomsEstimator.Domain.Operators;

public readonly record struct HomingOperator : IOperator
{
    public const double HomingDuration = 0.4;

    public string Symbol => "H";
}
