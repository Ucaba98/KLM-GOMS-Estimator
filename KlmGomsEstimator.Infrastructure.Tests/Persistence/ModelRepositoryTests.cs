using FluentAssertions;
using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Infrastructure.Options;
using KlmGomsEstimator.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace KlmGomsEstimator.Infrastructure.Tests.Persistence;

public class ModelRepositoryTests
{
    private readonly IJsonModelStorage _jsonModelStorage;
    private readonly ITextModelStorage _textModelStorage;
    private readonly IOptions<LocalStorageOptions> _storageOptions;

    public ModelRepositoryTests()
    {
        _jsonModelStorage = Substitute.For<IJsonModelStorage>();
        _textModelStorage = Substitute.For<ITextModelStorage>();
        _storageOptions = Substitute.For<IOptions<LocalStorageOptions>>();
    }

    [Fact]
    public void Save_CallsSaveOnBothStorages_ReturnsTrueIfBothSucceed()
    {
        // Arrange
        _storageOptions.Value.Returns(new LocalStorageOptions());

        var repository = new ModelRepository(_jsonModelStorage, _textModelStorage, _storageOptions);
        var model = new Model("Test Model");
        var fileName = "testFileName";
        var filePath = "testFilePath";
        var fullPath = $"{filePath}\\{fileName}";

        var typistSpeed = TypistSpeed.AverageTypist;
        _jsonModelStorage.Save(model, Arg.Any<string>()).Returns(true);
        _textModelStorage.Save(model, Arg.Any<TypistSpeed>(), Arg.Any<string>()).Returns(true);

        // Act
        var result = repository.Save(model, typistSpeed, fileName, filePath);

        // Assert
        result.Should().BeTrue();
        _jsonModelStorage.Received(1).Save(model, $"{fullPath}.json");
        _textModelStorage.Received(1).Save(model, typistSpeed, $"{fullPath}.txt");
    }

    [Fact]
    public void Save_ReturnsFalseIfJsonStorageFails()
    {
        // Arrange
        _storageOptions.Value.Returns(new LocalStorageOptions());

        var repository = new ModelRepository(_jsonModelStorage, _textModelStorage, _storageOptions);
        var model = new Model("Test Model");
        var fileName = "testFileName";
        var filePath = "testFilePath";
        var fullPath = $"{filePath}\\{fileName}";

        var typistSpeed = TypistSpeed.AverageTypist;
        _jsonModelStorage.Save(model, Arg.Any<string>()).Returns(false);
        _textModelStorage.Save(model, Arg.Any<TypistSpeed>(), Arg.Any<string>()).Returns(true);

        // Act
        var result = repository.Save(model, typistSpeed, fileName, filePath);

        // Assert
        result.Should().BeFalse();
        _jsonModelStorage.Received(1).Save(model, $"{fullPath}.json");
        _textModelStorage.DidNotReceive().Save(Arg.Any<Model>(), Arg.Any<TypistSpeed>(), Arg.Any<string>());
    }

    [Fact]
    public void Save_ReturnsFalseIfTextStorageFails()
    {
        // Arrange
        _storageOptions.Value.Returns(new LocalStorageOptions());

        var repository = new ModelRepository(_jsonModelStorage, _textModelStorage, _storageOptions);
        var model = new Model("Test Model");
        var fileName = "testFileName";
        var filePath = "testFilePath";
        var fullPath = $"{filePath}\\{fileName}";

        var typistSpeed = TypistSpeed.AverageTypist;
        _jsonModelStorage.Save(model, Arg.Any<string>()).Returns(true);
        _textModelStorage.Save(model, Arg.Any<TypistSpeed>(), Arg.Any<string>()).Returns(false);

        // Act
        var result = repository.Save(model, typistSpeed, fileName, filePath);

        // Assert
        result.Should().BeFalse();
        _jsonModelStorage.Received(1).Save(model, $"{fullPath}.json");
        _textModelStorage.Received(1).Save(model, typistSpeed, $"{fullPath}.txt");
    }

    [Fact]
    public void Save_UsesDefaultFilePathIfNull()
    {
        // Arrange
        var defaultFilePath = "defaultFilePath";
        _storageOptions.Value.Returns(new LocalStorageOptions { SavePath = defaultFilePath });

        var repository = new ModelRepository(_jsonModelStorage, _textModelStorage, _storageOptions);
        var model = new Model("Test Model");

        var fileName = "testFileName";
        var fullPath = $"{defaultFilePath}\\{fileName}";

        var typistSpeed = TypistSpeed.AverageTypist;
        _jsonModelStorage.Save(model, Arg.Any<string>()).Returns(true);
        _textModelStorage.Save(model, Arg.Any<TypistSpeed>(), Arg.Any<string>()).Returns(true);

        // Act
        var result = repository.Save(model, typistSpeed, fileName);

        // Assert
        result.Should().BeTrue();
        _jsonModelStorage.Received(1).Save(model, $"{fullPath}.json");
        _textModelStorage.Received(1).Save(model, typistSpeed, $"{fullPath}.txt");
    }

    [Fact]
    public void Load_CallsLoadOnJsonStorage_ReturnsLoadedModel()
    {
        // Arrange
        _storageOptions.Value.Returns(new LocalStorageOptions());

        var repository = new ModelRepository(_jsonModelStorage, _textModelStorage, _storageOptions);
        var fileName = "testFileName";
        var filePath = "testFilePath";
        var fullPath = $"{filePath}\\{fileName}.json";
        var expectedModel = new Model("Test Model");

        _jsonModelStorage.Load(fullPath).Returns(expectedModel);

        // Act
        var result = repository.Load(fileName, filePath);

        // Assert
        result.Should().Be(expectedModel);
        _jsonModelStorage.Received(1).Load(fullPath);
    }

    [Fact]
    public void Load_UsesDefaultFilePathIfNull()
    {
        // Arrange
        var defaultFilePath = "defaultFilePath";
        _storageOptions.Value.Returns(new LocalStorageOptions { SavePath = defaultFilePath });

        var repository = new ModelRepository(_jsonModelStorage, _textModelStorage, _storageOptions);
        var expectedModel = new Model("Test Model");

        var fileName = "testFileName";
        var fullPath = $"{defaultFilePath}\\{fileName}.json";

        _jsonModelStorage.Load(fullPath).Returns(expectedModel);

        // Act
        var result = repository.Load(fileName);

        // Assert
        result.Should().Be(expectedModel);
        _jsonModelStorage.Received(1).Load(fullPath);
    }
}
