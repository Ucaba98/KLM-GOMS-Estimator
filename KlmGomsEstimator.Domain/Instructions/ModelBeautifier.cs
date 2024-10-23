using KlmGomsEstimator.Domain.Duration;
using System.Text;

namespace KlmGomsEstimator.Domain.Instructions;

public class ModelBeautifier : IModelBeautifier
{
    private readonly IDurationCalculator _durationCalculator;

    public ModelBeautifier(IDurationCalculator durationCalculator)
    {
        _durationCalculator = durationCalculator;
    }

    public string BeautifyModel(Model model, TypistSpeed typistSpeed)
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"Model: {model.Description}");
        stringBuilder.AppendLine($"Typist Speed: {typistSpeed}");
        stringBuilder.AppendLine($"KLM-GOMS: {model.GetKlmCode()}");
        stringBuilder.AppendLine($"Duration: {_durationCalculator.CalculateDuration(model, typistSpeed)}s");

        for (var i = 0; i < model.Instructions.Count; i++)
        {
            var instruction = model.Instructions[i];

            stringBuilder.AppendLine($"\tInstruction {i + 1}:");
            stringBuilder.Append(BeautifyInstruction(instruction, typistSpeed, 1));
        }

        return stringBuilder.ToString();
    }

    public string BeautifyModel(Model model)
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"Model: {model.Description}");
        stringBuilder.AppendLine($"KLM-GOMS: {model.GetKlmCode()}");

        for (var i = 0; i < model.Instructions.Count; i++)
        {
            var instruction = model.Instructions[i];

            stringBuilder.AppendLine($"\tInstruction {i + 1}:");
            stringBuilder.Append(BeautifyInstruction(instruction, 1));
        }

        return stringBuilder.ToString();
    }

    public string BeautifyInstruction(Instruction instruction, TypistSpeed typistSpeed, int indentationLevel = 0)
    {
        StringBuilder stringBuilder = new();
        string indentation = GetIndentation(indentationLevel);

        stringBuilder.AppendLine($"{indentation}{instruction.Description}");
        stringBuilder.AppendLine($"{indentation}KLM-GOMS: {instruction.GetKlmCode()}");
        stringBuilder.AppendLine($"{indentation}Duration: {_durationCalculator.CalculateDuration(instruction, typistSpeed)}s");
        stringBuilder.AppendLine($"{indentation}Steps:");
        for (var j = 0; j < instruction.Steps.Count; j++)
        {
            var step = instruction.Steps[j];

            stringBuilder.AppendLine($"{indentation}\tStep {j + 1}:");
            stringBuilder.AppendLine(BeautifyStep(step, typistSpeed, indentationLevel + 1));
        }

        return stringBuilder.ToString();
    }

    public string BeautifyInstruction(Instruction instruction, int indentationLevel = 0)
    {
        StringBuilder stringBuilder = new();
        string indentation = GetIndentation(indentationLevel);

        stringBuilder.AppendLine($"{indentation}{instruction.Description}");
        stringBuilder.AppendLine($"{indentation}KLM-GOMS: {instruction.GetKlmCode()}");
        stringBuilder.AppendLine($"{indentation}Steps:");
        for (var j = 0; j < instruction.Steps.Count; j++)
        {
            var step = instruction.Steps[j];

            stringBuilder.AppendLine($"{indentation}\tStep {j + 1}:");
            stringBuilder.AppendLine(BeautifyStep(step, indentationLevel + 1));
        }

        return stringBuilder.ToString();
    }

    public string BeautifyStep(Step step, TypistSpeed typistSpeed, int indentationLevel = 0)
    {
        return $"{GetIndentation(indentationLevel)}{step.Description,-30} {step.Operator.Symbol,-5} = {_durationCalculator.CalculateDuration(step, typistSpeed)}s";
    }

    public string BeautifyStep(Step step, int indentationLevel = 0)
    {
        return $"{GetIndentation(indentationLevel)}{step.Description,-30} {step.Operator.Symbol}";
    }

    private static string GetIndentation(int indentationLevel) => new('\t', indentationLevel);
}
