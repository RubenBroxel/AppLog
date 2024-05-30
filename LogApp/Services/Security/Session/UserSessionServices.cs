using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.Security;
public class UserSessionServices: IUserSessionServices
{

    private HttpClient _httpClient;
    public UserSessionServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetTokenServiceAsync()
    {
        string? TokenLog = null;
        try
        {
            string  usuario = "{ 'Username' : 'John Tobbias' , 'Password': '1234' }";
            _httpClient.DefaultRequestHeaders.Clear();
            using dynamic jsonString = JObject.Parse(usuario);
            using var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8,"application/json");
            using var response =  await _httpClient.PostAsync("api/auth/token",httpContent);
            
            if(response.IsSuccessStatusCode)
            {
                var jsonresponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<JsonLogToken>(jsonresponse);
                TokenLog = result?.TokenLog;
            }
        
        }catch(HttpRequestException ex)
        {   
            TokenLog = ex.Message.ToString();
            
            #if DEBUG
                Console.WriteLine(ex.Message.ToString());
            #endif
        }
        return TokenLog;
    }



}