using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public interface IInstructionMenu
{
    Instruction CreateInstruction();
    void ModifyInstruction(Instruction instruction);
}