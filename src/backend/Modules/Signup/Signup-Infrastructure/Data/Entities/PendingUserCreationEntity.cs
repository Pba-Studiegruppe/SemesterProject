
namespace Signup_Infrastructure.Data.Entities;

internal class PendingUserCreationEntity
{
    public Guid Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool EmailWasSent { get; set; }
    public string Password { get; set; } = string.Empty;
}
