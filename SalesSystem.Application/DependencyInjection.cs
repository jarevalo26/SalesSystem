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
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBusinessService, BusinessService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISalesDocumentTypeService, SalesDocumentTypeService>();
        services.AddScoped<ISalesService, SalesService>();
        return services;
    }
}