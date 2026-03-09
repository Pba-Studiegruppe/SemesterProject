

using Microsoft.Extensions.Logging;
using Shared.Kernel;
using Signup_Application.Services;
using Signup_Domain.Models;
using Signup_Infrastructure.Data;
using Signup_Infrastructure.Data.Entities;

namespace Signup_Infrastructure.Services;

internal class SignupService : ISignupService
{
    private readonly ILogger<SignupService> _logger;
    private readonly SignupDbContext _dbContext;

    public SignupService(ILogger<SignupService> logger, SignupDbContext dbContext)
    {
        _logger = logger;
        this._dbContext = dbContext;
    }
    public async Task<SignupRequestResult> SignupAsync(SignupRequest request)
    {
        if (!SignupRequestIsValid(request))
            return SignupRequestResult.InvalidData();

        var entity = new PendingUserCreationEntity
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            Password = request.Password,
        };

        try
        {
            _dbContext.PendingUsers.Add(entity);
            await _dbContext.SaveChangesAsync();
            return SignupRequestResult.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Failed to save signupRequest to database");
            return SignupRequestResult.Failure();
        }
    }

    private bool SignupRequestIsValid(SignupRequest request)
    {
        if (request.FirstName.Length < 2 ||
            request.LastName.Length < 2 ||
            request.FirstName.Length > 40 ||
            request.LastName.Length > 40 ||
            !HelperMehods.LooksLikeEmail(request.Email))
            return false;
        return true;
    }
}
