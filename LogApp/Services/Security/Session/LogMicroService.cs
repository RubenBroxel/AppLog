using System.Text;
using Newtonsoft.Json;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.Security;

/// <summary>
/// Servicio para interactuar con el microservicio de autenticación y obtener tokens.
/// </summary>
public class LogMicroService : IUserSessionServices
{
    private readonly string UrlServiceAccount = "http://10.100.8.2:8484/";
    private readonly HttpClient _httpClient;

    public LogMicroService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(UrlServiceAccount);
    }

    /// <summary>
    /// Obtiene un token de autenticación del servicio.
    /// </summary>
    /// <param name="userCredentials">Credenciales del usuario para la autenticación.</param>
    /// <returns>Un <see cref="Task{TResult}"/> que representa la operación asíncrona. 
    /// El resultado es el token de autenticación o null si falla la autenticación.</returns>
    public async Task<string?> GetTokenServiceAsync(ModelAccountService userCredentials)
    {
        try
        {
            var userJson = new
            {
                AppService = userCredentials.AppService,
                AppToken = userCredentials.AppToken,
                AppName = userCredentials.AppName,
                AppPackage = userCredentials.AppPackage,
                AppBuild = userCredentials.AppBuild,
                AppVersion = userCredentials.AppVersion,
                AppCountry = userCredentials.AppCountry,
                AppIpAddress = userCredentials.AppIpAddress,
                AppLocation = userCredentials.AppLocation?.ToString(),
            };

            var jsonContent = JsonConvert.SerializeObject(userJson);
            using var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			_httpClient.DefaultRequestHeaders.Accept.Clear();
            using var response = await _httpClient.PostAsync("api/auth/token", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonresponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LogModelToken>(jsonresponse);
                return result?.TokenLog;
            }

            // Manejar errores del servidor
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error al obtener el token: {errorMessage},  {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            // Manejo centralizado de excepciones de red
            // Puedes registrar la excepción aquí o propagarla según tus necesidades.
            System.Diagnostics.Debug.WriteLine($"Error de red: {ex.Message}"); 
            return null;
        }
    }
}