using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThirdParty
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(Global.Config.IdentityMode.ToString("F"));

            using (var client = new HttpClient())
            {
                if (Global.Config.IdentityMode == Global.IdentityMode.ClientCredentials)
                {
                    var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                    {
                        Address = "http://localhost:5000/connect/token",
                        ClientId = "client",
                        ClientSecret = "secret",
                        Scope = "api1"
                    });
                    if (tokenResponse.IsError)
                        Console.WriteLine(tokenResponse.Error);
                    else
                        Console.WriteLine(tokenResponse.Json);
                    client.SetBearerToken(tokenResponse.AccessToken);

                    var apiResponse = await client.GetAsync("http://localhost:5002/api/values");
                    if (apiResponse.IsSuccessStatusCode)
                        Console.WriteLine(await apiResponse.Content.ReadAsStringAsync());
                    else
                        Console.WriteLine(apiResponse.StatusCode);
                }
                else if (Global.Config.IdentityMode == Global.IdentityMode.OwnerPassword)
                {
                    var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                    {
                        Address = "http://localhost:5000/connect/token",
                        ClientId = "client",
                        // ClientSecret = "secret",
                        Scope = "api1",
                        UserName = "admin1",
                        Password = "admin1"
                    });
                    if (tokenResponse.IsError)
                        Console.WriteLine(tokenResponse.Error);
                    else
                        Console.WriteLine(tokenResponse.Json);
                    client.SetBearerToken(tokenResponse.AccessToken);

                    var apiResponse = await client.GetAsync("http://localhost:5002/api/values");
                    if (apiResponse.IsSuccessStatusCode)
                        Console.WriteLine(await apiResponse.Content.ReadAsStringAsync());
                    else
                        Console.WriteLine(apiResponse.StatusCode);
                }
                else if (Global.Config.IdentityMode == Global.IdentityMode.AuthorizationCode)
                {

                }

            }

            Console.ReadKey();
        }
    }
}
