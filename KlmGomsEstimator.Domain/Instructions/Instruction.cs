namespace KlmGomsEstimator.Domain.Instructions;

public class Instruction(string description)
{
    public string Description { get; set; } = description;

    private readonly List<Step> _steps = [];

    public List<Step> Steps => _steps;

    public void AddStep(Step step)
    {
        _steps.Add(step);
    }

    public bool RemoveStep(int index)
    {
        if (index < 0 || index >= _steps.Count)
        {
            return false;
        }

        _steps.RemoveAt(index);

        return true;
    }

    public bool MoveStep(int startIndex, int moveIndex)
    {
        if (moveIndex < 0 || moveIndex >= _steps.Count)
        {
            return false;
        }

        if (startIndex == moveIndex)
        {
            return true;
        }

        var step = _steps[startIndex];
        _steps.RemoveAt(startIndex);
        _steps.Insert(moveIndex, step);

        return true;
    }

    public string GetKlmCode() => string.Join(string.Empty, _steps.Select(s => s.Operator.Symbol));
}
