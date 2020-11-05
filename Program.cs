using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace AzureGraphApiTry
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var configuration = builder.Build();
            var azureAdConfiguration = configuration.GetSection("AzureAd");
            var options = new ConfidentialClientApplicationOptions()
            {
                ClientId = azureAdConfiguration["ClientId"],
                ClientSecret = azureAdConfiguration["ClientSecret"],
                Instance = azureAdConfiguration["Instance"],
                TenantId = azureAdConfiguration["TenantId"]
            };

            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .CreateWithApplicationOptions(options)
                .Build();
            var authenticationResult = await confidentialClientApplication.AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();
            var accessToken = authenticationResult.AccessToken;

            Console.WriteLine(accessToken);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://graph.microsoft.com")
            };
            httpClient.SetBearerToken(accessToken);
            using var request = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri("v1.0/users", UriKind.RelativeOrAbsolute)
            };

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, default);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.ToString());
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(responseStream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var serializer = JsonSerializer.Create();
            var userReponse = serializer.Deserialize<UserResponse>(jsonTextReader);
            var users = userReponse.Users;
            users.ForEach(user =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));
            });

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
