using System.Text;
using Newtonsoft.Json;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.Security;
public class UserSessionServices: IUserSessionServices
{
    private readonly HttpClient _httpClient;
    public UserSessionServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetTokenServiceAsync(UserCredentials userCredentials)
    {
        string? TokenLog = null;
        try
        {        
            var userJson = new { Username = "John Tobbias", Password = "1234" };
            var jsonContent = JsonConvert.SerializeObject(userJson);

            // Crear el contenido HTTP directamente con el JSON serializado
            using var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Realizar la solicitud POST
            using var response = await _httpClient.PostAsync("api/auth/token", httpContent);

            // Verificar el estado de la respuesta
            if (response.IsSuccessStatusCode)
            {
                // Deserializar la respuesta directamente en el objeto deseado
                var jsonresponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<JsonLogToken>(jsonresponse);
                TokenLog = result?.TokenLog;
            }
            else
            {
                // Manejar errores del servidor
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener el token: {errorMessage}");
            }
        
        }catch(HttpRequestException ex)
        {   
            #if DEBUG
                Console.WriteLine(ex.Message.ToString());
            #endif
            return TokenLog = ex.Message.ToString();
        }
        return TokenLog;
    }



}