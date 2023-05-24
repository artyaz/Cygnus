using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Cygnus.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class NovaPoshtaApiOptions
{
    public string ApiKey { get; set; }
}
public class MethodProperties
{
    public string CityName { get; set; }
}

public class RequestContent
{
    public string apiKey { get; set; }
    public string modelName { get; set; }
    public string calledMethod { get; set; }
    public MethodProperties methodProperties { get; set; }
}
public class NovaPoshtaApi
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NovaPoshtaApi(HttpClient httpClient, IOptions<NovaPoshtaApiOptions> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }

    public async Task<List<string>> GetCitiesAsync()
    {
        var requestContent = new
        {
            apiKey = _apiKey,
            modelName = "Address",
            calledMethod = "getCities",
            methodProperties = new { }
        };

        var response = await _httpClient.PostAsJsonAsync("https://api.novaposhta.ua/v2.0/json/", requestContent);
        var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, JsonElement>>();
        var cities = responseObject["data"].EnumerateArray().Select(city => city.GetProperty("Description").ToString()).ToList();
        return cities;
    }

    public async Task<List<string>> GetPostDepartmentsAsync(string cityName)
    {
        var requestContent = new
        {
            apiKey = _apiKey,
            modelName = "Address",
            calledMethod = "getWarehouses",
            methodProperties = new { CityName = cityName }
        };

        // Serialize the request content manually
        var jsonRequest = JsonSerializer.Serialize(requestContent);
        var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.novaposhta.ua/v2.0/json/", httpContent);
        var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, JsonElement>>();
        var postDepartments = responseObject["data"].EnumerateArray().Select(pd => pd.GetProperty("Description").ToString()).ToList();
        return postDepartments;
    }
}