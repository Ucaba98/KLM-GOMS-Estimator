using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Infrastructure.Dtos;
using System.Text.Json;

namespace KlmGomsEstimator.Infrastructure.Persistence;

public class JsonModelStorage : IJsonModelStorage
{
    private static readonly JsonSerializerOptions _jsonSerializationOptions = new() { WriteIndented = true };

    public bool Save(Model model, string filePath)
    {
        try
        {
            var modelDto = model.ToJsonDto();
            var json = JsonSerializer.Serialize(modelDto, _jsonSerializationOptions);
            File.WriteAllText(filePath, json);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public Model? Load(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            var modelDto = JsonSerializer.Deserialize<ModelJsonDto>(json, _jsonSerializationOptions);

            return modelDto!.ToModel();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
