using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity;

public class EasyIdentityOptions
{
    public IList<SigningCredentials> SigningCredentials { get; set; }

    public EasyIdentityOptions()
    {
        SigningCredentials = new List<SigningCredentials>();
    }
}
