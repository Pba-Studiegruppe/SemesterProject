

using Signup_Domain.Models;

namespace Signup_Application.Services;

public interface ISignupService
{
    Task<SignupRequestResult> SignupAsync(SignupRequest request);
}
