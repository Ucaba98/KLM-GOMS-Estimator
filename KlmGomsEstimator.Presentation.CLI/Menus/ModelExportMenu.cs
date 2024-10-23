using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Infrastructure.Persistence;
using KlmGomsEstimator.Presentation.CLI.Terminal;

namespace KlmGomsEstimator.Presentation.CLI.Menus;

public class ModelExportMenu : IModelExportMenu
{
    private readonly IConsole _console;
    private readonly IModelRepository _modelRepository;

    public ModelExportMenu(IConsole console, IModelRepository modelRepository)
    {
        _console = console;
        _modelRepository = modelRepository;
    }

    public void ExportModel(Model model, TypistSpeed typistSpeed)
    {
        var exportingModel = true;
        while (exportingModel)
        {
            _console.WriteLine("Export model (y/n)?");
            var exportAction = _console.ReadLine()?.Trim().ToLower();

            switch (exportAction)
            {
                case "y":
                    var fileName = _console.ReadNonNullString("File name?", "File name?");

                    if (_modelRepository.Save(model, typistSpeed, fileName))
                    {
                        _console.WriteLine("Model exported");
                        exportingModel = false;
                    }
                    else
                    {
                        _console.WriteLine("Model could not be exported");
                    }

                    break;
                case "n":
                    exportingModel = false;
                    break;
            }
        }
    }

    public Model? ImportModel()
    {
        string? fileName = null;
        while (fileName is null)
        {
            fileName = _console.ReadNonNullString("Please write the file name (.json).", "File name?");
            if (!fileName.EndsWith(".json"))
            {
                _console.WriteLine("File must have the '.json' extension");
                fileName = null;
            }
        }

        return _modelRepository.Load(fileName);
    }
}
