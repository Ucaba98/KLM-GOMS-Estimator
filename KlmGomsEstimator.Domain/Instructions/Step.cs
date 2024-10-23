using KlmGomsEstimator.Domain.Operators;

namespace KlmGomsEstimator.Domain.Instructions;

public record Step(string Description, IOperator Operator);
