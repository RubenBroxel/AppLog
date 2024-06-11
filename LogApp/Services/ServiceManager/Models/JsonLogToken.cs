using Newtonsoft.Json;

namespace LogApp.Services.ServicesManager.Models;
public class LogModelToken
{
    [JsonProperty("token")]
    public string? TokenLog { get; set; }
}