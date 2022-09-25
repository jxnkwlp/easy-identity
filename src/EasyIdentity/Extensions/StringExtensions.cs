namespace EasyIdentity.Extensions;

internal static class StringExtensions
{
    public static string EnsureEndsWith(this string value, char c)
    {
        if (value?.EndsWith(c) == true)
            return value;
        return value + c;
    }
}
