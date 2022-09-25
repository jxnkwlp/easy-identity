using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using IdentityModel.OidcClient;
using Microsoft.Net.Http.Server;

namespace SimpleOidcClientConsoleApp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        OidcClient client = new OidcClient(new OidcClientOptions
        {
            Authority = "https://localhost:44377/",
            ClientId = "client1",
            ClientSecret = "1234567890",
            Scope = "openid offline_access",
            LoadProfile = false,
            FilterClaims = true,
            RedirectUri = "http://127.0.0.1:7890/",
        });

        var state = await client.PrepareLoginAsync();

        Console.WriteLine($"Try load url {state.StartUrl}");

        OpenBrowser(state.StartUrl);

        // create a redirect URI using an available port on the loopback address.
        string redirectUri = string.Format("http://127.0.0.1:7890/");

        // create an HttpListener to listen for requests on that redirect URI.
        var settings = new WebListenerSettings();
        settings.UrlPrefixes.Add(redirectUri);
        var http = new WebListener(settings);

        http.Start();
        Console.WriteLine("Listening..");

        var context = await http.AcceptAsync();

        context.Response.ContentType = "text/plain";
        context.Response.StatusCode = 200;

        var buffer = Encoding.UTF8.GetBytes("Login completed. Please return to the console application.");
        await context.Response.Body.WriteAsync(buffer);
        await context.Response.Body.FlushAsync();

        context.Dispose();

        var result = await client.ProcessResponseAsync(context.Request.QueryString, state);

        if (result.IsError)
        {
            Console.WriteLine("An error occurred: {0}", result.Error);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Claims:");

            foreach (var claim in result.User.Claims)
            {
                Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Access token:");
            Console.WriteLine();
            Console.WriteLine(result.AccessToken);
        }

    }

    public static void OpenBrowser(string url)
    {
        try
        {
            Process.Start(url);
        }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }
    }
}
