
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Signup_Infrastructure.Data;
namespace SignupModule.Tests;

public class SignupModuleWebApplicationFactory: WebApplicationFactory<Program>
{
    public string ConnectionString { get; set; } = default!; // will be set from fixture


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SignupDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<SignupDbContext>(options =>
            {
                options.UseNpgsql(ConnectionString);
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SignupDbContext>();

            db.Database.EnsureCreated();
        });
    }
}
