using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;

namespace KlmGomsEstimator.Infrastructure.Dtos;

public static class ModelMappingExtensions
{
    public static ModelJsonDto ToJsonDto(this Model model)
    {
        return new ModelJsonDto(
            model.Description,
            model.Instructions.Select(i => i.ToJsonDto()).ToList()
        );
    }

    public static Model ToModel(this ModelJsonDto modelDto)
    {
        var model = new Model(modelDto.Description);

        foreach (var instructionDto in modelDto.Instructions)
        {
            model.AddInstruction(instructionDto.ToModel());
        }

        return model;
    }

    private static InstructionJsonDto ToJsonDto(this Instruction instruction)
    {
        return new InstructionJsonDto(
            instruction.Description,
            instruction.Steps.Select(s => s.ToJsonDto()).ToList()
        );
    }

    private static StepJsonDto ToJsonDto(this Step step)
    {
        return new StepJsonDto(
            step.Description,
            step.Operator switch
            {
                ButtonPressOperator => "B",
                HomingOperator => "H",
                MentalPreparationOperator => "M",
                PointingOperator => "P",
                SystemResponseTimeOperator => "R",
                KeystrokeOperator k => $"{k.Keystrokes}K{(int)k.Complexity}",

                _ => throw new InvalidOperationException("Unknown operator type.")
            }
        );
    }

    private static Instruction ToModel(this InstructionJsonDto instructionDto)
    {
        var instruction = new Instruction(instructionDto.Description);

        foreach (var stepDto in instructionDto.Steps)
        {
            instruction.AddStep(stepDto.ToModel());
        }

        return instruction;
    }

    private static Step ToModel(this StepJsonDto stepDto)
    {
        IOperator op = stepDto.OperatorCode switch
        {
            "B" => new ButtonPressOperator(),
            "H" => new HomingOperator(),
            "M" => new MentalPreparationOperator(),
            "P" => new PointingOperator(),
            "R" => new SystemResponseTimeOperator(),

            _ => ConvertKeystrokeCodeToOperator(stepDto.OperatorCode)
        };

        return new Step(stepDto.Description, op);
    }

    private static KeystrokeOperator ConvertKeystrokeCodeToOperator(string operatorCode)
    {
        try
        {
            var segments = operatorCode.Split('K');
            int keystrokes = int.Parse(segments[0]);
            var complexity = (KeystrokeComplexity)int.Parse(segments[1]);

            return new KeystrokeOperator(keystrokes, complexity);
        }
        catch
        {
            throw new InvalidOperationException("Unknown operator type.");
        }
    }
}
