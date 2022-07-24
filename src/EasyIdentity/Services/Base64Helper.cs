using System;

namespace EasyIdentity.Services;

public static class Base64Helper
{
    public static bool Compare(string source1, string source2)
    {
        //source1 = source1.PadRight(source1.Length + (4 - source1.Length % 4) % 4, '=');
        //source2 = source2.PadRight(source2.Length + (4 - source2.Length % 4) % 4, '=');

        source1 = source1.TrimEnd('=');
        source2 = source2.TrimEnd('=');

        return string.Equals(source1, source2);
    }

    public static string ToBase64String(byte[] source)
    {
        var result = Convert.ToBase64String(source);
        return result.Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }
}
