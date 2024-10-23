using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Infrastructure.Persistence;

public interface IModelRepository
{
    bool Save(Model model, TypistSpeed typistSpeed, string fileName, string? filePath = null);
    Model? Load(string fileName, string? filePath = null);
}
