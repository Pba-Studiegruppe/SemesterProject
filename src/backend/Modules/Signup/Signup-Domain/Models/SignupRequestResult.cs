
namespace Signup_Domain.Models;

public record SignupRequestResult
{
    public SignupRequestStatus Status { get; init; } = SignupRequestStatus.Undefined;
    public enum SignupRequestStatus
    {
        Undefined,
        Success,
        InvalidData,
        Failure
    }

    public static SignupRequestResult Success() => new SignupRequestResult { Status = SignupRequestStatus.Success };
    public static SignupRequestResult InvalidData() => new SignupRequestResult { Status = SignupRequestStatus.InvalidData };
    public static SignupRequestResult Failure() => new SignupRequestResult
    {
        Status = SignupRequestStatus.Failure
    };

}