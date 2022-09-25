using System.IdentityModel.Tokens.Jwt;
using EasyIdentity.Extensions;
using EasyIdentity.Models;
using EasyIdentityServerBasic;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer()
    ;

var identityBuilder = builder.Services.AddEasyIdentity(option => { });

identityBuilder.AddClient(new EasyIdentity.Models.Client
{
    ClientId = "client1",
    ClientSecret = "1234567890",
    DisplayName = "client1",
    GrantTypes = GrantTypeNameConsts.All,
    Scopes = new string[] { StandardScopes.OpenId, StandardScopes.OfflineAccess, StandardScopes.Email, StandardScopes.Profile },
    Enabled = true,
    RedirectUrls = new string[] { "" }
})
.AddStandardScopes()
.AddDevelopmentRSACredentialsStore()
.AddUserService<TestUserService>()
;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseEasyIdentity();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
