namespace KlmGomsEstimator.Domain.Instructions;

public class Model(string description)
{
    public string Description { get; set; } = description;

    private readonly List<Instruction> _instructions = [];

    public List<Instruction> Instructions => _instructions;

    public void AddInstruction(Instruction instruction)
    {
        _instructions.Add(instruction);
    }

    public bool RemoveInstruction(int index)
    {
        if (index < 0 || index >= _instructions.Count)
        {
            return false;
        }

        _instructions.RemoveAt(index);

        return true;
    }

    public bool MoveInstruction(int startIndex, int moveIndex)
    {
        if (moveIndex < 0 || moveIndex >= _instructions.Count)
        {
            return false;
        }

        if (startIndex == moveIndex)
        {
            return true;
        }

        var instruction = _instructions[startIndex];
        _instructions.RemoveAt(startIndex);
        _instructions.Insert(moveIndex, instruction);

        return true;
    }

    public object GetKlmCode() => string.Join(string.Empty, _instructions.Select(i => i.GetKlmCode()));
}
