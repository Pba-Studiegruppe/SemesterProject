
namespace Shared.Kernel;

public static class HelperMehods
{
    public static bool LooksLikeEmail(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        int atIndex = input.IndexOf('@');
        int dotIndex = input.LastIndexOf('.');

        return atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < input.Length - 1;
    }
}
