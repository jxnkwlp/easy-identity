using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace DeviceCodeConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        { 
            IdentityModelEventSource.ShowPII = true;

            HttpClient httpClient = new HttpClient();

            var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7139/");

            if (discoveryDocumentResponse.IsError)
                Console.WriteLine(discoveryDocumentResponse.Error);

            var response = await httpClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
            {
                ClientId = "client1",
                Scope = "openid offline_access profile email",
                RequestUri = new Uri(discoveryDocumentResponse.DeviceAuthorizationEndpoint),
            });

            if (response.IsError)
                Console.WriteLine(response.Error);

            Console.WriteLine(response.Raw);

            //var securityKey = new RsaSecurityKey(RSA.Create(2048)) { KeyId = Guid.NewGuid().ToString("N") };
            //var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKeyS"));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //// var encryptingCredentials = new EncryptingCredentials(key, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256);
            // var encryptingCredentials = new EncryptingCredentials(securityKey, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes256CbcHmacSha512);


            //var handler = new JsonWebTokenHandler();


            //var id = new System.Security.Claims.ClaimsIdentity("abc");
            //id.AddClaim(new System.Security.Claims.Claim("sub", "123456"));


            ////var token = handler.CreateToken(new SecurityTokenDescriptor()
            ////{
            ////    Issuer = "http://baidu.com",
            ////    Subject = id,
            ////    SigningCredentials = signingCredentials,
            ////    EncryptingCredentials = encryptingCredentials
            ////});

            //var tokenDescriptor = new SecurityTokenDescriptor()
            //{
            //    Issuer = "http://baidu.com",
            //    Audience = "321321",
            //    Subject = id,
            //    IssuedAt = DateTime.Now,
            //    NotBefore = DateTime.Now.AddSeconds(60),
            //    Expires = DateTime.Now.AddSeconds(60),
            //    SigningCredentials = signingCredentials,
            //    EncryptingCredentials = encryptingCredentials,
            //};

            //var jwe = handler.CreateToken("{}", signingCredentials, encryptingCredentials);

            //Console.WriteLine(jwe);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //var jwt = tokenHandler.WriteToken(securityToken);

            //Console.WriteLine(jwt);

            //var payload = handler.CreateToken(new SecurityTokenDescriptor()
            //{
            //    Issuer = "http://baidu.com",
            //    Subject = id, 
            //});


            // Console.WriteLine(payload);

            //var payload = JsonSerializer.Serialize(new { sub = "123456" });

            //var token = handler.CreateToken(tokenDescriptor);

            //Console.WriteLine(token);

            //Console.WriteLine();

            //var jwe = handler.EncryptToken(token, encryptingCredentials);

            //Console.WriteLine(jwe);





            //var payload = new JObject()
            //    {
            //        { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
            //        { JwtRegisteredClaimNames.GivenName, "Bob" },
            //        { JwtRegisteredClaimNames.Iss, "http://Default.Issuer.com" },
            //        { JwtRegisteredClaimNames.Aud, "http://Default.Audience.com" },
            //        { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Parse("2017-03-17T18:33:37.095Z")).ToString() },
            //        { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(DateTime.Parse("2017-03-17T18:33:37.080Z")).ToString() },
            //        { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.Parse("2021-03-17T18:33:37.080Z")).ToString() },
            //    }.ToString();

            //var jsonWebTokenHandler = new JsonWebTokenHandler();
            //var signingCredentials = Default.SymmetricSigningCredentials;
            //var encryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256);
            //var jwe = jsonWebTokenHandler.CreateToken(payload, signingCredentials, encryptingCredentials);







        }
    }
}
