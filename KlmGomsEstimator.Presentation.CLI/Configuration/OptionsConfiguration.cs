using KlmGomsEstimator.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KlmGomsEstimator.Presentation.CLI.Configuration;

public static class OptionsConfiguration
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services
            .AddOptions<LocalStorageOptions>()
            .BindConfiguration(LocalStorageOptions.CONFIGURATION_SECTION)
            .ReplacePlaceholders();

        return services;
    }

    private static void ReplacePlaceholders(this OptionsBuilder<LocalStorageOptions> optionsBuilder)
    {
        optionsBuilder.PostConfigure(options =>
        {
            options.SavePath = options.SavePath.Replace(
                "%DESKTOP%",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                StringComparison.InvariantCultureIgnoreCase);
        });
    }
}
