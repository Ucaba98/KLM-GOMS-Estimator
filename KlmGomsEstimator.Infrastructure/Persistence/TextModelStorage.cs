using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Infrastructure.Persistence;

public class TextModelStorage : ITextModelStorage
{
    private readonly IModelBeautifier _modelBeautifier;

    public TextModelStorage(IModelBeautifier modelBeautifier)
    {
        _modelBeautifier = modelBeautifier;
    }

    public bool Save(Model model, TypistSpeed typistSpeed, string filePath)
    {
        try
        {
            var formattedModel = _modelBeautifier.BeautifyModel(model, typistSpeed);
            File.WriteAllText(filePath, formattedModel);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}
