using FluentAssertions;
using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Infrastructure.Persistence;
using KlmGomsEstimator.Presentation.CLI.Menus;
using KlmGomsEstimator.Presentation.CLI.Terminal;
using NSubstitute;

namespace KlmGomsEstimator.Presentation.CLI.Tests.Menus;

public class ModelExportMenuTests
{
    private readonly IConsole _consoleMock;
    private readonly IModelRepository _modelRepositoryMock;
    private readonly ModelExportMenu _modelExportMenuSut;

    public ModelExportMenuTests()
    {
        _consoleMock = Substitute.For<IConsole>();
        _modelRepositoryMock = Substitute.For<IModelRepository>();
        _modelExportMenuSut = new ModelExportMenu(_consoleMock, _modelRepositoryMock);
    }

    [Fact]
    public void Constructor_InitializesDependencies()
    {
        // Assert
        _modelExportMenuSut.Should().NotBeNull();
    }

    [Fact]
    public void ExportModel_UserChoosesYes_ModelIsExported()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var model = new Model("Test Model");
        var fileName = "test_model.json";

        _consoleMock.ReadLine().Returns("y", "n");
        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(fileName);
        _modelRepositoryMock.Save(model, typistSpeed, fileName).Returns(true);

        // Act
        _modelExportMenuSut.ExportModel(model, typistSpeed);

        // Assert
        _consoleMock.Received().WriteLine("Export model (y/n)?");
        _consoleMock.Received().WriteLine("Model exported");
        _modelRepositoryMock.Received().Save(model, typistSpeed, fileName);
    }

    [Fact]
    public void ExportModel_UserChoosesNo_ModelIsNotExported()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var model = new Model("Test Model");

        _consoleMock.ReadLine().Returns("n");

        // Act
        _modelExportMenuSut.ExportModel(model, typistSpeed);

        // Assert
        _consoleMock.Received().WriteLine("Export model (y/n)?");
        _modelRepositoryMock.DidNotReceive().Save(Arg.Any<Model>(), Arg.Any<TypistSpeed>(), Arg.Any<string>());
    }

    [Fact]
    public void ExportModel_SaveFails_ModelCouldNotBeExported()
    {
        // Arrange
        var typistSpeed = TypistSpeed.AverageTypist;
        var model = new Model("Test Model");
        var fileName = "test_model.json";

        _consoleMock.ReadLine().Returns("y", "n");
        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(fileName);
        _modelRepositoryMock.Save(model, typistSpeed, fileName).Returns(false);

        // Act
        _modelExportMenuSut.ExportModel(model, typistSpeed);

        // Assert
        _consoleMock.Received().WriteLine("Export model (y/n)?");
        _consoleMock.Received().WriteLine("Model could not be exported");
        _modelRepositoryMock.Received().Save(model, typistSpeed, fileName);
    }

    [Fact]
    public void ImportModel_ValidJsonFile_ModelIsImported()
    {
        // Arrange
        var fileName = "valid_model.json";
        var model = new Model("Test Model");

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(fileName);
        _modelRepositoryMock.Load(fileName).Returns(model);

        // Act
        var result = _modelExportMenuSut.ImportModel();

        // Assert
        _modelRepositoryMock.Received().Load(fileName);
        result.Should().Be(model);
    }

    [Fact]
    public void ImportModel_InvalidFileExtension_ShowsErrorMessage()
    {
        // Arrange
        var invalidFileName = "invalid_model.txt";
        var validFileName = "valid_model.json";
        var model = new Model("Test Model");

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(invalidFileName, validFileName);
        _modelRepositoryMock.Load(validFileName).Returns(model);

        // Act
        var result = _modelExportMenuSut.ImportModel();

        // Assert
        _consoleMock.Received().WriteLine("File must have the '.json' extension");
        _modelRepositoryMock.Received().Load(validFileName);
        result.Should().Be(model);
    }

    [Fact]
    public void ImportModel_EmptyFileName_ShowsErrorMessage()
    {
        // Arrange
        var emptyFileName = "";
        var validFileName = "valid_model.json";
        var model = new Model("Test Model");

        _consoleMock.ReadNonNullString(Arg.Any<string>(), Arg.Any<string>()).Returns(emptyFileName, validFileName);
        _modelRepositoryMock.Load(validFileName).Returns(model);

        // Act
        var result = _modelExportMenuSut.ImportModel();

        // Assert
        _consoleMock.Received().WriteLine("File must have the '.json' extension");
        _modelRepositoryMock.Received().Load(validFileName);
        result.Should().Be(model);
    }
}
