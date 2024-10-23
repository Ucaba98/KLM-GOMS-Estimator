using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Presentation.CLI.Terminal;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public class InstructionMenu : IInstructionMenu
{
    private readonly IConsole _console;
    private readonly IStepMenu _stepMenu;
    private readonly IModelBeautifier _modelBeautifier;

    public InstructionMenu(IConsole console, IStepMenu stepReaderMenu, IModelBeautifier modelBeautifier)
    {
        _console = console;
        _stepMenu = stepReaderMenu;
        _modelBeautifier = modelBeautifier;
    }

    public Instruction ReadInstruction()
    {
        var description = _console.ReadNonNullString(
            "Let's add an instruction. Please write a description:",
            "Please write a description for the instruction:");

        Instruction instruction = new(description);

        AddStep(instruction);
        ModifyInstruction(instruction);

        return instruction;
    }

    public void ModifyInstruction(Instruction instruction)
    {
        var arrangingSteps = true;
        while (arrangingSteps)
        {
            _console.WriteLine(_modelBeautifier.BeautifyInstruction(instruction));
            _console.WriteLine("Add step (a), remove step (r), move step (m), change description (d), or quit (q)?");

            var stepAction = _console.ReadLine()?.Trim().ToLower();
            switch (stepAction)
            {
                case "a":
                    AddStep(instruction);
                    break;
                case "r":
                    RemoveStep(instruction);
                    break;
                case "m":
                    MoveStep(instruction);
                    break;
                case "d":
                    ChangeInstructionDescription(instruction);
                    break;
                case "q":
                    arrangingSteps = false;
                    break;
            }
        }
    }

    private void AddStep(Instruction instruction)
    {
        instruction.AddStep(_stepMenu.ReadStep());
        _console.WriteLine("Step added");
    }

    private void RemoveStep(Instruction instruction)
    {
        var chosenIndex = _console.ReadIndex("Choose step to remove:", 1, instruction.Steps.Count);

        if (instruction.RemoveStep(chosenIndex))
        {
            _console.WriteLine($"Step {chosenIndex + 1} removed");
        }
        else
        {
            _console.WriteLine($"Step {chosenIndex + 1} does not exist");
        }
    }

    private void MoveStep(Instruction instruction)
    {
        var startIndex = _console.ReadIndex("Choose step to move:", 1, instruction.Steps.Count);
        var moveIndex = _console.ReadIndex("Choose where to move the step:", 1, instruction.Steps.Count);

        if (instruction.MoveStep(startIndex, moveIndex))
        {
            _console.WriteLine($"Step {startIndex + 1} moved to position {moveIndex + 1}");
        }
        else
        {
            _console.WriteLine($"Step {startIndex + 1} could not be moved to position {moveIndex + 1}");
        }
    }

    private void ChangeInstructionDescription(Instruction instruction)
    {
        var newDescription = _console.ReadNonNullString(
            "Please write a new description for the instruction:",
            "Please write a new description for the instruction:");

        instruction.Description = newDescription;
        _console.WriteLine("Description changed.");
    }
}
