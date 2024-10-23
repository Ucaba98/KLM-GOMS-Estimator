using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace KlmGomsEstimator.Infrastructure.Persistence;

public class ModelRepository : IModelRepository
{
    private readonly IJsonModelStorage _jsonModelStorage;
    private readonly ITextModelStorage _textModelStorage;
    private readonly LocalStorageOptions _storageOptions;

    public ModelRepository(IJsonModelStorage jsonModelStorage, ITextModelStorage textModelStorage, IOptions<LocalStorageOptions> storageOptions)
    {
        _jsonModelStorage = jsonModelStorage;
        _textModelStorage = textModelStorage;
        _storageOptions = storageOptions.Value;
    }

    public bool Save(Model model, TypistSpeed typistSpeed, string fileName, string? filePath = null)
    {
        filePath ??= _storageOptions.SavePath;
        filePath = Path.Combine(filePath, fileName);

        if (!_jsonModelStorage.Save(model, $"{filePath}.json"))
        {
            return false;
        }

        if (!_textModelStorage.Save(model, typistSpeed, $"{filePath}.txt"))
        {
            return false;
        }

        return true;
    }

    public Model? Load(string fileName, string? filePath = null)
    {
        filePath ??= _storageOptions.SavePath;
        fileName = fileName.EndsWith(".json") ? fileName : $"{fileName}.json";
        filePath = Path.Combine(filePath, fileName);

        return _jsonModelStorage.Load(filePath);
    }
}
