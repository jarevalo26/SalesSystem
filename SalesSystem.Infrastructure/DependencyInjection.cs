using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Application.Interfaces;
using SalesSystem.Infrastructure.Contexts;
using SalesSystem.Infrastructure.Implementations;

namespace SalesSystem.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<VentasDbContext>(options =>
            options.UseSqlServer(defaultConnectionString));

        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ISaleRepository, SaleRepository>();

        return services;
    }
}