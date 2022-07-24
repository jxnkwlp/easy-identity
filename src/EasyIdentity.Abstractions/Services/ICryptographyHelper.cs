namespace EasyIdentity.Services;

public interface ICryptographyHelper
{
    byte[] Sha256(byte[] source);
}
