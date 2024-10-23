namespace KlmGomsEstimator.Presentation.CLI.Terminal;

public class SystemConsole : IConsole
{
    public void WriteLine() => Console.WriteLine();

    public void WriteLine(string message) => Console.WriteLine(message);

    public void WriteLine(object obj) => Console.WriteLine(obj);

    public void Write(string message) => Console.Write(message);

    public void Write(object obj) => Console.Write(obj);

    public string? ReadLine() => Console.ReadLine();

    public int ReadIndex(string instruction, int minValue, int maxValue)
    {
        int? chosenIndex = null;

        while (chosenIndex is null)
        {
            WriteLine(instruction);
            var choice = ReadLine();

            if (int.TryParse(choice, out var index) && index >= minValue && index <= maxValue)
            {
                chosenIndex = index - 1;
                continue;
            }

            WriteLine("Invalid choice. Please try again.");
        }

        return (int)chosenIndex;
    }

    public string ReadNonNullString(string instruction, string repeatedInstruction)
    {
        WriteLine(instruction);

        var text = ReadLine();

        while (text is null)
        {
            WriteLine(repeatedInstruction);
            text = ReadLine();
        }

        return text;
    }
}
