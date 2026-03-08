using Microsoft.AspNetCore.Mvc;
using Signup_Application.Services;
using Signup_Domain.Models;
using static Signup_Domain.Models.SignupRequestResult;

namespace AppHost;

public static class Routes
{
    public static WebApplication MapRoutes(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        app.MapGet("/api/signup", async ([FromServices]ISignupService signupService, [FromBody]SignupRequest request) => 
        {
            var result = await signupService.SignupAsync(request);
            if (result.Status == SignupRequestStatus.Success)
                return Results.Ok();
            else if (result.Status is SignupRequestStatus.InvalidData)
                return Results.BadRequest();
            else 
                return Results.BadRequest();          
        });

        return app;
    }
}