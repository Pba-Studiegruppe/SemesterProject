
namespace Signup_Domain.Models;

public record SignupRequest
{
    public string LastName { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}
