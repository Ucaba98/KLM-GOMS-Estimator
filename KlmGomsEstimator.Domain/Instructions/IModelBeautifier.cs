using KlmGomsEstimator.Domain.Duration;

namespace KlmGomsEstimator.Domain.Instructions;

public interface IModelBeautifier
{
    string BeautifyModel(Model model, TypistSpeed typistSpeed);
    string BeautifyModel(Model model);
    string BeautifyInstruction(Instruction instruction, TypistSpeed typistSpeed, int indentationLevel = 0);
    string BeautifyInstruction(Instruction instruction, int indentationLevel = 0);
    string BeautifyStep(Step step, TypistSpeed typistSpeed, int indentationLevel = 0);
    string BeautifyStep(Step step, int indentationLevel = 0);
}