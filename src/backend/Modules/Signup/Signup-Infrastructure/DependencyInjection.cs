using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Signup_Application.Services;
using Signup_Infrastructure.Data;
using Signup_Infrastructure.Services;

namespace Signup_Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSignupInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ISignupService, SignupService>();
        services.AddDbContext<SignupDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("signupmoduleDbConnectionString")));

        return services;
    }
}
