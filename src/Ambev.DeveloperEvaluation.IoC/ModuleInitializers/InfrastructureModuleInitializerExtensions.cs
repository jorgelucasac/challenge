using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
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

            if (!pendingMigrations.Any())
            {
                logger.LogInformation("No pending database migrations were found.");
                return;
            }

            await context.Database.MigrateAsync();
            logger.LogInformation(
                "Database migrations applied successfully. PendingCount={PendingCount}",
                pendingMigrations.Count());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database migrations could not be applied during startup. The application will continue running.");
        }
    }

    public static async Task SeedDataSafelyAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DefaultContext>>();

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            if (!await context.Users.AnyAsync())
            {
                context.Users.Add(new User
                {
                    Username = "Admin User",
                    Email = "admin@developerstore.local",
                    Phone = "(11) 99999-9999",
                    Password = passwordHasher.HashPassword("Admin@123"),
                    Role = UserRole.Admin,
                    Status = UserStatus.Active
                });
            }

            if (!await context.Sales.AnyAsync())
            {
                context.Sales.AddRange(
                    Sale.Create(
                        "SALE-0001",
                        DateTime.UtcNow.AddDays(-2),
                        "customer-seed-1",
                        "John Doe",
                        "branch-seed-1",
                        "Sao Paulo",
                        [
                            new SaleItemInput("product-seed-1", "Laptop Pro 14", 1, 7500m),
                            new SaleItemInput("product-seed-2", "Wireless Mouse", 4, 150m)
                        ]),
                    Sale.Create(
                        "SALE-0002",
                        DateTime.UtcNow.AddDays(-1),
                        "customer-seed-2",
                        "Mary Smith",
                        "branch-seed-2",
                        "Rio de Janeiro",
                        [
                            new SaleItemInput("product-seed-3", "Mechanical Keyboard", 2, 550m),
                            new SaleItemInput("product-seed-4", "USB-C Dock", 5, 320m)
                        ]),
                    Sale.Create(
                        "SALE-0003",
                        DateTime.UtcNow,
                        "customer-seed-3",
                        "Acme Corp",
                        "branch-seed-3",
                        "Belo Horizonte",
                        [
                            new SaleItemInput("product-seed-5", "4K Monitor", 10, 1800m)
                        ]));
            }

            if (!await context.Products.AnyAsync())
            {
                context.Products.AddRange(
                    Product.Create(
                        "Fjallraven Backpack",
                        109.95m,
                        "Everyday backpack with laptop compartment.",
                        "bags",
                        "https://fakestore.local/images/backpack.png",
                        4.6m,
                        120),
                    Product.Create(
                        "Slim Fit T-Shirt",
                        22.30m,
                        "Soft cotton t-shirt for daily wear.",
                        "men's clothing",
                        "https://fakestore.local/images/tshirt.png",
                        4.2m,
                        98),
                    Product.Create(
                        "Women's Jacket",
                        55.99m,
                        "Lightweight jacket with modern fit.",
                        "women's clothing",
                        "https://fakestore.local/images/jacket.png",
                        4.8m,
                        76),
                    Product.Create(
                        "Wireless Headphones",
                        199.99m,
                        "Noise-cancelling headphones with long battery life.",
                        "electronics",
                        "https://fakestore.local/images/headphones.png",
                        4.7m,
                        210));
            }

            if (!context.ChangeTracker.HasChanges())
            {
                logger.LogInformation("Seed data skipped because the database already contains records.");
                return;
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Seed data applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Seed data could not be applied during startup. The application will continue running.");
        }
    }
}
