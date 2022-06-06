using EasyIdentity.Extensions;
using EasyIdentity.Models;
using EasyIdentityBasic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Logging;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddEasyIdentity(options =>
{
    options.AddClient(new EasyIdentity.Models.Client
    {
        ClientId = "client1",
        ClientSecret = "1234567890",
        ClientSecretRequired = true,
        ClientName = "client1",
        GrantTypes = GrantTypesConsts.All,
        Scopes = new string[] { StandardScopes.OpenId, StandardScopes.Email, StandardScopes.Profile, StandardScopes.OfflineAccess },
        Enabled = true,
        RedirectUrls = new string[] { "" }
    });
    options.AddStandardScopes();
    options.AddDevelopmentSigningCredentials();

    options.AddUserProfileService<TestUserProfileService>();
});


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

app.UseAuthentication();

app.UseRouting();

app.UseEasyIdentity();

app.UseAuthorization();

app.MapRazorPages();

app.MapDefaultControllerRoute();

app.Run();
