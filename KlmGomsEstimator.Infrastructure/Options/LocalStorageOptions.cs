namespace KlmGomsEstimator.Infrastructure.Options;

public class LocalStorageOptions
{
    public const string CONFIGURATION_SECTION = "LocalStorage";

    public string SavePath { get; set; } = string.Empty;
}
