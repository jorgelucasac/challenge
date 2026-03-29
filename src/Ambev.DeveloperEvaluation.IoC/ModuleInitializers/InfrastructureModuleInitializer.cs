using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        AddDataBase(builder);
        AddRepositories(builder);
    }

    private static void AddDataBase(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DefaultContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b =>
                {
                    b.EnableRetryOnFailure();
                    b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM");
                }
            );

            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.LogTo(Console.WriteLine, LogLevel.Information);
            }
        });
    }

    private static void AddRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISaleNumberGenerator, DefaultSaleNumberGenerator>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }
}
