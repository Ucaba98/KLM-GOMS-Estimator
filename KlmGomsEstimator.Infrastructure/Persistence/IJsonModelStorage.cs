using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Infrastructure.Persistence;
public interface IJsonModelStorage
{
    Model? Load(string filePath);
    bool Save(Model model, string filePath);
}