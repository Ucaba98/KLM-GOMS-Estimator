using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Presentation.CLI.Menus.Options;
using KlmGomsEstimator.Presentation.CLI.Terminal;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public class InstructionMenu : IInstructionMenu
{
    private readonly IConsole _console;
    private readonly IStepMenu _stepMenu;
    private readonly IModelBeautifier _modelBeautifier;

    private readonly OptionsMenu _modifyMenu;

    private static readonly TerminalOption _seeDetails = new("See Details", 's');
    private static readonly TerminalOption _addStep = new("Add Step", 'a');
    private static readonly TerminalOption _removeStep = new("Remove Step", 'r');
    private static readonly TerminalOption _moveStep = new("Move Step", 'm');
    private static readonly TerminalOption _changeDescription = new("Change Instruction Description", 'd');
    private static readonly TerminalOption _quit = TerminalOption.Quit;

    private readonly Dictionary<TerminalOption, Action<Instruction>> _optionActions;

    public InstructionMenu(IConsole console, IStepMenu stepReaderMenu, IModelBeautifier modelBeautifier)
    {
        _console = console;
        _stepMenu = stepReaderMenu;
        _modelBeautifier = modelBeautifier;

        _optionActions = new()
        {
            { _seeDetails, DisplayInstruction },
            { _addStep, AddStep },
            { _removeStep, RemoveStep },
            { _moveStep, MoveStep },
            { _changeDescription, ChangeInstructionDescription }
        };

        _modifyMenu = new("How would you like to modify this instruction?", _console)
        {
            _seeDetails,
            _addStep,
            _removeStep,
            _moveStep,
            _changeDescription,
            _quit
        };
    }

    public Instruction CreateInstruction()
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
        while (true)
        {
            DisplayInstruction(instruction);

            var choice = _modifyMenu.ReadOption();

            if (choice.IsQuit)
            {
                break;
            }

            _optionActions[choice](instruction);
        }
    }

    private void AddStep(Instruction instruction)
    {
        instruction.AddStep(_stepMenu.CreateStep());
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

    private void DisplayInstruction(Instruction instruction) => _console.WriteLine(_modelBeautifier.BeautifyInstruction(instruction));
}
