using KlmGomsEstimator.Presentation.CLI.Terminal;
using System.Collections;

namespace KlmGomsEstimator.Presentation.CLI.Menus.Options;

public class OptionsMenu : IEnumerable<TerminalOption>
{
    private readonly List<TerminalOption> _options = [];

    private readonly string _requestToUser;
    private readonly IConsole _console;

    public OptionsMenu(string requestToUser, IConsole console)
    {
        _requestToUser = requestToUser;
        _console = console;
    }

    public IEnumerator<TerminalOption> GetEnumerator() => _options.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(TerminalOption option)
    {
        if (_options.Any(_options => _options.Shortkey == option.Shortkey))
        {
            throw new InvalidOperationException($"Option with shortkey {option.Shortkey} already exists.");
        }

        _options.Add(option);
    }

    public TerminalOption ReadOption()
    {
        if (_options.Count == 0)
        {
            throw new InvalidOperationException("No options available.");
        }

        while (true)
        {
            Display();
            var input = _console.ReadLine();

            if (int.TryParse(input, out var numericChoice) && numericChoice > 0 && numericChoice <= _options.Count)
            {
                return _options[numericChoice - 1];
            }

            if (TryParseCharOption(input, out int choice))
            {
                return _options[choice];
            }

            _console.WriteLine("Invalid choice. Please try again.");
        }
    }

    private bool TryParseCharOption(string? input, out int choice)
    {
        choice = -1;
        if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
        {
            return false;
        }

        var charChoice = char.ToLowerInvariant(input[0]);

        for (var i = 0; i < _options.Count; i++)
        {
            TerminalOption? option = _options[i];
            if (char.ToLowerInvariant(option.Shortkey) == charChoice)
            {
                choice = i;
                return true;
            }
        }

        return false;
    }

    private void Display()
    {
        _console.WriteLine(_requestToUser);
        for (int i = 0; i < _options.Count; i++)
        {
            var option = _options[i];
            _console.WriteLine($"{i + 1}. {option.Text} [{option.Shortkey}]");
        }
    }
}