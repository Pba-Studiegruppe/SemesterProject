using Microsoft.Extensions.DependencyInjection;
using Signup_Application.Services;
using Signup_Infrastructure.Services;

namespace Signup_Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSignupInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISignupService, SignupService>();
        return services;
    }
}
