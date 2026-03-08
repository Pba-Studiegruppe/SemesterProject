

using Microsoft.Extensions.Logging;
using Signup_Application.Services;
using Signup_Domain.Models;

namespace Signup_Infrastructure.Services; 

internal class SignupService : ISignupService
{
    private readonly ILogger<SignupService> _logger;

    public SignupService(ILogger<SignupService> logger)
    {
        _logger = logger;
    }
    public Task<SignupRequestResult> SignupAsync(SignupRequest request)
    {
        _logger.LogInformation("Received signup request for {Email}", request.Email);
        return Task.FromResult(SignupRequestResult.Success());
    }
}
