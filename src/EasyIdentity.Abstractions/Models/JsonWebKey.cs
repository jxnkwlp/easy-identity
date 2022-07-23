namespace EasyIdentity.Models;

public class JsonWebKey
{
    public string Use { get; set; }
    public string Kid { get; set; }
    public string Kty { get; set; }
    public string Alg { get; set; }
    public string E { get; set; }
    public string N { get; set; }
    public string X { get; set; }
    public string Y { get; set; }
    public string Crv { get; set; }
    public string X5t { get; set; }
    public string[] X5c { get; set; }
}
