using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebAppInsights.Models;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace WebAppInsights.Controllers;

[ApiController]
public class SlowAPI : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SlowAPI(IHttpClientFactory httpClientFactory) =>
       _httpClientFactory = httpClientFactory;        

    [HttpGet]
    [Route("api/SlowAPIGet")]
    public async Task<String> SlowAPIGet()
    {
        // Random Delay
        Random random = new Random();
        int ranNum = random.Next(1,30000);
        Console.WriteLine("Delaying in: "+ranNum);
        Task.Delay(ranNum).Wait();

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "https://api.github.com/repos/welasco/HubitatAlarm")
        {
            Headers =
            {
                { HeaderNames.Accept, "application/vnd.github.v3+json" },
                { HeaderNames.UserAgent, "HttpRequestsSample" }
            }
        };

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);        
        
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            Console.WriteLine("Test");
            
            var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(jsonString);
            return jsonString;
            //Console.WriteLine(System.Text.Encoding.UTF8.GetString(contentStream.Read()));
        }

        return "Fail";
    }

}
