using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Presentation.CLI.Terminal;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public class ModelMenu : IModelMenu
{
    private readonly IConsole _console;
    private readonly IInstructionMenu _instructionMenu;

    public ModelMenu(IConsole console, IInstructionMenu instructionMenu)
    {
        _console = console;
        _instructionMenu = instructionMenu;
    }

    public Model ReadModel()
    {
        _console.WriteLine("You will now input your KLM-GOMS model.");
        var description = _console.ReadNonNullString(
            "Please write a description for the model:",
            "Please write a description for the model:");

        Model model = new(description);

        AddInstruction(model);
        ModifyMenu(model);

        return model;
    }

    public void ModifyMenu(Model model)
    {
        var arrangingInstructions = true;
        while (arrangingInstructions)
        {
            _console.WriteLine("Add instruction (a), remove instruction (r), move instruction (m), change description (d), or quit (q)?");

            var action = _console.ReadLine()?.Trim().ToLower();
            switch (action)
            {
                case "a":
                    AddInstruction(model);
                    break;
                case "r":
                    RemoveInstruction(model);
                    break;
                case "m":
                    MoveInstruction(model);
                    break;
                case "d":
                    ChangeModelDescription(model);
                    break;
                case "q":
                    arrangingInstructions = false;
                    break;
            }
        }
    }

    private void AddInstruction(Model model)
    {
        model.AddInstruction(_instructionMenu.ReadInstruction());
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

    private void ChangeModelDescription(Model model)
    {
        var newDescription = _console.ReadNonNullString(
            "Please write a new description for the model:",
            "Please write a new description for the model:");

        model.Description = newDescription;
        _console.WriteLine("Description changed.");
    }
}