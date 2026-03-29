using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public static class InfrastructureModuleInitializerExtensions
{
    public static async Task ApplyMigrationsSafelyAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DefaultContext>>();

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying database migrations...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied successfully.");
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database migrations could not be applied during startup. The application will continue running.");
        }
    }
}