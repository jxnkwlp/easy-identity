namespace EasyIdentity.Services;

internal class CryptographyHelper : ICryptographyHelper
{
    public byte[] Sha256(byte[] source)
    {
#if NET5_0_OR_GREATER
        return System.Security.Cryptography.SHA256.HashData(source);
#else
        using var hasher = System.Security.Cryptography.SHA256.Create();
        return hasher.ComputeHash(source);
#endif
    }
}
