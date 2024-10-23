using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public interface IInstructionMenu
{
    Instruction ReadInstruction();
    void ModifyInstruction(Instruction instruction);
}