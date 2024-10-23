namespace KlmGomsEstimator.Domain.Operators;

public readonly record struct MentalPreparationOperator : IOperator
{
    public const double MentalPreparationDuration = 1.35;

    public string Symbol => "M";
}
