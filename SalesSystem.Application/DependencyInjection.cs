using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Application.Implementation;
using SalesSystem.Application.Interfaces;

namespace SalesSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IFireBaseService, FireBaseService>();
        services.AddScoped<IUtilitiesService, UtilitiesService>();
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
}