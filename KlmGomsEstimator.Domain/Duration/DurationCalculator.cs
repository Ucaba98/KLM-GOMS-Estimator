using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;

namespace KlmGomsEstimator.Domain.Duration;

public class DurationCalculator : IDurationCalculator
{
    public double CalculateDuration(Model model, TypistSpeed typistSpeed)
    {
        return model.Instructions.Sum(instruction => CalculateDuration(instruction, typistSpeed));
    }

    public double CalculateDuration(Instruction instruction, TypistSpeed typistSpeed)
    {
        return instruction.Steps.Sum(step => CalculateDuration(step, typistSpeed));
    }

    public double CalculateDuration(Step step, TypistSpeed typistSpeed)
    {
        return CalculateDuration(step.Operator, typistSpeed);
    }

    public double CalculateDuration(IOperator op, TypistSpeed typistSpeed)
    {
        return op switch
        {
            ButtonPressOperator => ButtonPressOperator.ButtonPressDuration,
            HomingOperator => HomingOperator.HomingDuration,
            MentalPreparationOperator => MentalPreparationOperator.MentalPreparationDuration,
            PointingOperator => PointingOperator.PointingDuration,
            SystemResponseTimeOperator => SystemResponseTimeOperator.SystemResponseTimeDuration,
            KeystrokeOperator keystrokeOp => CalculateKeystrokeDuration(keystrokeOp, typistSpeed),

            _ => throw new NotImplementedException(),
        };
    }

    private static double CalculateKeystrokeDuration(KeystrokeOperator keystrokeOp, TypistSpeed typistSpeed)
        => keystrokeOp.Complexity switch
        {
            KeystrokeComplexity.Regular => keystrokeOp.Keystrokes * typistSpeed,
            KeystrokeComplexity.Random => keystrokeOp.Keystrokes * KeystrokeOperator.RandomLetterTypingSpeed,
            KeystrokeComplexity.Complex => keystrokeOp.Keystrokes * KeystrokeOperator.ComplexCodeTypingSpeed,

            _ => throw new NotImplementedException(),
        };
}
