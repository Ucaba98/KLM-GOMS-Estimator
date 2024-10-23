using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Presentation.CLI.Terminal;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public class TypistSpeedMenu : ITypistSpeedMenu
{
    private static readonly TypistSpeed[] _typistSpeeds =
    [
        TypistSpeed.BestTypist,
        TypistSpeed.GoodTypist,
        TypistSpeed.AverageTypist,
        TypistSpeed.AverageNonSecretaryTypist,
        TypistSpeed.WorstTypist,
    ];

    private readonly IConsole _console;

    public TypistSpeedMenu(IConsole console)
    {
        _console = console;
    }

    public TypistSpeed ChooseTypistSpeed()
    {
        DisplayTypistSpeeds();
        var typistChoice = GetTypistSpeedChoice();

        _console.WriteLine($"Chosen typist level: {typistChoice}");

        return typistChoice;
    }

    private void DisplayTypistSpeeds()
    {
        _console.WriteLine("Typist speeds:");
        for (var i = 0; i < _typistSpeeds.Length; i++)
        {
            _console.WriteLine($"{i + 1}. {_typistSpeeds[i]}");
        }
    }

    private TypistSpeed GetTypistSpeedChoice()
    {
        var chosenIndex = _console.ReadIndex("Choose a typist speed:", 1, _typistSpeeds.Length);

        return _typistSpeeds[chosenIndex];
    }
}
