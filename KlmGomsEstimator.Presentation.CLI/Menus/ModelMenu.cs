using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Presentation.CLI.Menus.Options;
using KlmGomsEstimator.Presentation.CLI.Terminal;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public class ModelMenu : IModelMenu
{
    private readonly IConsole _console;
    private readonly IInstructionMenu _instructionMenu;
    private readonly IModelBeautifier _modelBeautifier;

    private readonly OptionsMenu _modifyMenu;

    private static readonly TerminalOption _seeDetails = new("See Details", 's');
    private static readonly TerminalOption _addInstruction = new("Add Instruction", 'a');
    private static readonly TerminalOption _removeInstruction = new("Remove Instruction", 'r');
    private static readonly TerminalOption _moveInstruction = new("Move Instruction", 'm');
    private static readonly TerminalOption _modifyInstruction = new("Modify Instruction", 'i');
    private static readonly TerminalOption _changeDescription = new("Change Model Description", 'd');
    private static readonly TerminalOption _quit = TerminalOption.Quit;

    private readonly Dictionary<TerminalOption, Action<Model>> _optionActions;

    public ModelMenu(IConsole console, IInstructionMenu instructionMenu, IModelBeautifier modelBeautifier)
    {
        _console = console;
        _instructionMenu = instructionMenu;
        _modelBeautifier = modelBeautifier;

        _optionActions = new()
        {
            { _seeDetails, DisplayModel },
            { _addInstruction, AddInstruction },
            { _removeInstruction, RemoveInstruction },
            { _moveInstruction, MoveInstruction },
            { _modifyInstruction, ModifyInstruction },
            { _changeDescription, ChangeModelDescription }
        };

        _modifyMenu = new("How would you like to modify this instruction?", _console)
        {
            _seeDetails,
            _addInstruction,
            _removeInstruction,
            _moveInstruction,
            _modifyInstruction,
            _changeDescription,
            _quit
        };
    }

    public Model CreateModel()
    {
        _console.WriteLine("You will now input your KLM-GOMS model.");
        var description = _console.ReadNonNullString(
            "Please write a description for the model:",
            "Please write a description for the model:");

        Model model = new(description);

        AddInstruction(model);
        ModifyModel(model);

        return model;
    }

    public void ModifyModel(Model model)
    {
        while (true)
        {
            var choice = _modifyMenu.ReadOption();

            if (choice.IsQuit)
            {
                break;
            }

            _optionActions[choice](model);
        }
    }

    private void AddInstruction(Model model)
    {
        model.AddInstruction(_instructionMenu.CreateInstruction());
        _console.WriteLine("Instruction added");
    }

    private void RemoveInstruction(Model model)
    {
        var chosenIndex = _console.ReadIndex("Choose instruction to remove:", 1, model.Instructions.Count);

        if (model.RemoveInstruction(chosenIndex))
        {
            _console.WriteLine($"Instruction {chosenIndex + 1} removed");
        }
        else
        {
            _console.WriteLine($"Instruction {chosenIndex + 1} does not exist");
        }
    }

    private void MoveInstruction(Model model)
    {
        var startIndex = _console.ReadIndex("Choose instruction to move:", 1, model.Instructions.Count);
        var moveIndex = _console.ReadIndex("Choose where to move the instruction:", 1, model.Instructions.Count);

        if (model.MoveInstruction(startIndex, moveIndex))
        {
            _console.WriteLine($"Instruction {startIndex + 1} moved to {moveIndex + 1}");
        }
        else
        {
            _console.WriteLine($"Instruction {startIndex + 1} could not be moved to {moveIndex + 1}");
        }
    }

    private void ModifyInstruction(Model model)
    {
        var chosenIndex = _console.ReadIndex("Choose instruction to modify:", 1, model.Instructions.Count);
        _instructionMenu.ModifyInstruction(model.Instructions[chosenIndex]);
    }

    private void ChangeModelDescription(Model model)
    {
        var newDescription = _console.ReadNonNullString(
            "Please write a new description for the model:",
            "Please write a new description for the model:");

        model.Description = newDescription;
        _console.WriteLine("Description changed.");
    }

    private void DisplayModel(Model model) => _console.WriteLine(_modelBeautifier.BeautifyModel(model));
}
