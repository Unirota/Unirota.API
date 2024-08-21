using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Unirota.Migrations;

public static class Startup
{
    public static void AddMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(
                config => config
                    .AddPostgres()
                    .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(Assembly.GetExecutingAssembly())
                    .For.All()
                    .WithGlobalCommandTimeout(new TimeSpan(0, 2, 0))
            )
            .AddLogging(config => config.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    public static void AddMigrationRunner(this IServiceProvider servicesProvider)
    {
        var runner = servicesProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
