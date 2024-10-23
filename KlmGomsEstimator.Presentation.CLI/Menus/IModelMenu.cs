using KlmGomsEstimator.Domain.Instructions;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public interface IModelMenu
{
    Model ReadModel();
    void ModifyMenu(Model model);
}