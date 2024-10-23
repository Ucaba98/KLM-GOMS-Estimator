using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public interface IModelExportMenu
{
    void ExportModel(Model model, TypistSpeed typistSpeed);
    Model? ImportModel();
}
