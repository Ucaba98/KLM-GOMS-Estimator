namespace KlmGomsEstimator.Presentation.CLI.Terminal;

public interface IConsole
{
    void WriteLine();
    void WriteLine(string message);
    void WriteLine(object obj);
    void Write(string message);
    void Write(object obj);
    string? ReadLine();
    int ReadIndex(string instruction, int minValue, int maxValue);
    string ReadNonNullString(string instruction, string repeatedInstruction);
}
