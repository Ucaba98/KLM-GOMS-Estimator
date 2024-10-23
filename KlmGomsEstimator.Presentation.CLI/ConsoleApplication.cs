
using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Presentation.CLI.Menus;
using KlmGomsEstimator.Presentation.CLI.Terminal;

internal class ConsoleApplication
{
    private TypistSpeed _currentTypingSpeed;

    private readonly IConsole _console;
    private readonly ITypistSpeedMenu _typistSpeedMenu;
    private readonly IModelMenu _modelMenu;
    private readonly IModelExportMenu _modelExportMenu;
    private readonly IModelBeautifier _modelBeautifier;

    public ConsoleApplication(
        IConsole console,
        ITypistSpeedMenu typistChooser,
        IModelMenu modelReader,
        IModelExportMenu modelExporter,
        IModelBeautifier modelBeautifier)
    {
        _console = console;
        _typistSpeedMenu = typistChooser;
        _modelMenu = modelReader;
        _modelExportMenu = modelExporter;
        _modelBeautifier = modelBeautifier;
    }

    public void Run()
    {
        _currentTypingSpeed = _typistSpeedMenu.ChooseTypistSpeed();
        Console.WriteLine();

        bool quit = false;
        while (!quit)
        {
            var action = _console.ReadNonNullString("Would you like to create a (n)ew model, (r)ead an existing model, or (q)uit?", "(n)ew model, (r)ead model, or (q)uit").Trim().ToLower();

            switch (action)
            {
                case "n":
                    CreateModel();
                    break;
                case "r":
                    ImportModel();
                    break;
                case "q":
                    quit = true;
                    break;
            }
        }
    }

    private void ImportModel()
    {
        Model? importedModel = null;
        while (importedModel is null)
        {
            importedModel = _modelExportMenu.ImportModel();

            if (importedModel is null)
            {
                _console.WriteLine("Model could not be imported");
            }
        }

        var formattedModel = _modelBeautifier.BeautifyModel(importedModel, _currentTypingSpeed);
        _console.WriteLine(formattedModel);

        _modelMenu.ModifyModel(importedModel);
        _modelExportMenu.ExportModel(importedModel, _currentTypingSpeed);
    }

    private void CreateModel()
    {
        var newModel = _modelMenu.CreateModel();
        var formattedModel = _modelBeautifier.BeautifyModel(newModel, _currentTypingSpeed);
        _console.WriteLine(formattedModel);

        _modelExportMenu.ExportModel(newModel, _currentTypingSpeed);
    }
}
