using Newtonsoft.Json;

namespace LogApp.Services.ServicesManager.Models;
public class JsonLogToken
{
    [JsonProperty("token")]
    public string? TokenLog { get; set; }
}