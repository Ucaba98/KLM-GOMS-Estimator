using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public interface IModelMenu
{
    Model CreateModel();
    void ModifyModel(Model model);
}