namespace EasyIdentity.Models;

public class Client
{
    public string ClientId { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; } = true;

    public string ClientSecret { get; set; }

    //public bool ClientSecretRequired { get; set; } = true;

    public string[] GrantTypes { get; set; }

    public string[] Scopes { get; set; }

    //public bool ConsentRequired { get; set; }

    public string BaseUrl { get; set; }

    public string[] RedirectUrls { get; set; }

}
