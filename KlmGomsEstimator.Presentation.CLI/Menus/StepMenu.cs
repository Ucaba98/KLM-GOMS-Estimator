using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Domain.Operators;
using KlmGomsEstimator.Presentation.CLI.Terminal;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public class StepMenu : IStepMenu
{
    private readonly IConsole _console;

    public StepMenu(IConsole console)
    {
        _console = console;
    }

    public Step ReadStep()
    {
        var step = _console.ReadNonNullString(
            "Let's add a step. Please write a description:",
            "Please write a description for the step:");

        IOperator op = ReadOperator();

        return new Step(step, op);
    }

    private IOperator ReadOperator()
    {
        _console.WriteLine("Please write an operator.");
        _console.WriteLine("[n]K - Keystroke; n - optional number of keystrokes (default = 1)");
        _console.WriteLine("M - Mental Preparation");
        _console.WriteLine("H - Homing");
        _console.WriteLine("P - Pointing");
        _console.WriteLine("B - Button Press");
        _console.WriteLine("R - System Response Time");
        IOperator? op = null;

        while (op is null)
        {
            var choice = _console.ReadLine()?.Trim().ToLower();

            if (choice is null)
            {
                continue;
            }

            if (choice.Length == 1)
            {
                op = choice switch
                {
                    "k" => new KeystrokeOperator(1, KeystrokeComplexity.Complex),
                    "m" => new MentalPreparationOperator(),
                    "h" => new HomingOperator(),
                    "p" => new PointingOperator(),
                    "b" => new ButtonPressOperator(),
                    "r" => new SystemResponseTimeOperator(),

                    _ => null
                };
            }
            else if (choice[^1] == 'k')
            {
                int? number = int.TryParse(choice[..^1], out var n) ? n : null;
                if (number is not null)
                {
                    _console.WriteLine("What complexity does the text have?");
                    _console.WriteLine("1. Regular");
                    _console.WriteLine("2. Random");
                    _console.WriteLine("3. Complex");

                    var complexity = _console.ReadIndex("Choose text complexity:", 1, 3);

                    op = new KeystrokeOperator((int)number, (KeystrokeComplexity)complexity);
                }
            }

            if (op is null)
            {
                _console.WriteLine("Invalid operator. Please try again.");
            }
        }

        return op;
    }
}
