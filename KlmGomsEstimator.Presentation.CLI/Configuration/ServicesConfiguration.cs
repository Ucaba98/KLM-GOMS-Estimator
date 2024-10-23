using KlmGomsEstimator.Domain.Duration;
using KlmGomsEstimator.Domain.Instructions;
using KlmGomsEstimator.Infrastructure.Persistence;
using KlmGomsEstimator.Presentation.CLI.Menus;
using KlmGomsEstimator.Presentation.CLI.Terminal;
using Microsoft.Extensions.DependencyInjection;

namespace KlmGomsEstimator.Presentation.CLI.Configuration;

internal static class ServicesConfiguration
{
    internal static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IJsonModelStorage, JsonModelStorage>();
        services.AddTransient<ITextModelStorage, TextModelStorage>();
        services.AddTransient<IModelRepository, ModelRepository>();

        services.AddScoped<IModelBeautifier, ModelBeautifier>();
        services.AddScoped<IDurationCalculator, DurationCalculator>();
        services.AddScoped<IConsole, SystemConsole>();

        services.AddScoped<IStepMenu, StepMenu>();
        services.AddScoped<IInstructionMenu, InstructionMenu>();
        services.AddScoped<ITypistSpeedMenu, TypistSpeedMenu>();
        services.AddScoped<IModelMenu, ModelMenu>();
        services.AddScoped<IModelExportMenu, ModelExportMenu>();

        services.AddSingleton<ConsoleApplication>();

        return services;
    }
}
