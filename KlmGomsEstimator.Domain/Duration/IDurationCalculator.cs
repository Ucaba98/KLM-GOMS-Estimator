using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;

namespace KlmGomsEstimator.Domain.Duration;
public interface IDurationCalculator
{
    double CalculateDuration(Instruction instruction, TypistSpeed typistSpeed);
    double CalculateDuration(IOperator op, TypistSpeed typistSpeed);
    double CalculateDuration(Model model, TypistSpeed typistSpeed);
    double CalculateDuration(Step step, TypistSpeed typistSpeed);
}