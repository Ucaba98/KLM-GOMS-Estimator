namespace KlmGomsEstimator.Presentation.CLI.Menus.Options;

public record TerminalOption(string Text, char Shortkey)
{
    public static readonly TerminalOption Quit = new("Quit", 'q');

    public bool IsQuit => this == Quit;
}
