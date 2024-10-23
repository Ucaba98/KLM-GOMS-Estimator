using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Infrastructure.Persistence;
public interface ITextModelStorage
{
    bool Save(Model model, TypistSpeed typistSpeed, string filePath);
}