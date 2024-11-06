using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Threading;

public class Worker : BackgroundService
{
    private readonly HttpClient _httpClient;
    public Worker(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _httpClient.GetStringAsync("http://yourapi.com/data.xml");
            
            // Convert XML to JSON
            var xml = XDocument.Parse(response);
            var json = JsonConvert.SerializeXNode(xml);

            // Process JSON data (assuming each item is an object in the JSON array)
            dynamic data = JsonConvert.DeserializeObject(json);
            foreach (var item in data.items) // Adjust this to match your JSON structure
            {
                // Process each item here
                Console.WriteLine(item);
            }

            // Wait for 1 minute before the next check
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
