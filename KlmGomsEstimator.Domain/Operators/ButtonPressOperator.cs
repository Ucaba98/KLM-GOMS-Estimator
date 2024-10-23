namespace KlmGomsEstimator.Domain.Operators;

public readonly record struct ButtonPressOperator : IOperator
{
    public const double ButtonPressDuration = 0.1;

    public readonly string Symbol => "B";
}
