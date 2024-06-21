using System.Text;
using Newtonsoft.Json;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.Security;

/// <summary>
/// Clase con responsabilidad de manejar el inicio de sesión para las cuentas de servicio.
/// </summary>
public class AccountMicroService : IAccountMicroService
{
    private readonly HttpClient _httpClient;
    private readonly string UrlServiceAccount = "http://10.100.8.2:5484/";

    public AccountMicroService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(UrlServiceAccount);
    }

    /// <summary>
    /// Obtiene un token de autenticación para una cuenta de servicio.
    /// </summary>
    /// <param name="userCredentials">Credenciales de la cuenta de servicio.</param>
    /// <returns>Un <see cref="Task{TResult}"/> que representa la operación asíncrona. 
    /// El resultado es el token de autenticación o una cadena vacía si ocurre un error.</returns>
    public async Task<string> GetTokenServiceAsync(ModelCredentialService userCredentials)
    {
        try
        {
            var userJson = new { Username = userCredentials.UserName, Password = userCredentials.UserPassword };
            var jsonContent = JsonConvert.SerializeObject(userJson);
            using var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			_httpClient.DefaultRequestHeaders.Accept.Clear();
            using var response = await _httpClient.PostAsync("api/auth/user", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonresponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LogModelToken>(jsonresponse);
                return result?.TokenLog ?? string.Empty;
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error al obtener el token: {errorMessage}", null, response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            // Manejo de errores de solicitud HTTP.
            // Puedes registrar la excepción o manejarla de otra manera según tus necesidades.
            System.Diagnostics.Debug.WriteLine($"Error en la solicitud HTTP: {ex.Message}");
            return string.Empty;
        }
        catch (Exception ex)
        {
            // Manejo de otras excepciones.
            System.Diagnostics.Debug.WriteLine($"Error al obtener el token: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Crea un nuevo objeto <see cref="AccountModelService"/> con información del dispositivo.
    /// </summary>
    /// <param name="token">Token de autenticación.</param>
    /// <returns>Un <see cref="Task{TResult}"/> que representa la operación asíncrona. 
    /// El resultado es un <see cref="AccountModelService"/> con la información del dispositivo.</returns>
    public async Task<ModelAccountService> CreateServiceAccountAsync(string token)
    {
        var cuentaServicio = new ModelAccountService(token);
        if (!string.IsNullOrEmpty(token))
        {
            cuentaServicio.AppIpAddress = await GetPublicIpAsync();
            cuentaServicio.AppCountry   = await GetCountryFromLocationAsync();
            cuentaServicio.AppLocation  = await GetLocationAsync();
        }
        return cuentaServicio;
    }

    private async Task<string> GetPublicIpAsync()
    {
        try
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync("https://api.ipify.org");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            // Manejo de excepciones al obtener la IP pública.
            System.Diagnostics.Debug.WriteLine($"Error al obtener la IP pública: {ex.Message}");
            return string.Empty;
        }
    }

    private async Task<Location?> GetLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);
            return location;
        }
        catch (Exception ex)
        {
            // Manejo de excepciones al obtener la ubicación.
            System.Diagnostics.Debug.WriteLine($"Error al obtener la ubicación: {ex.Message}");
            return null;
        }
    }

    private async Task<string> GetCountryFromLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                return placemark?.CountryName ?? "País no encontrado";
            }

            return "No se pudo obtener la ubicación.";
        }
        catch (Exception ex)
        {
            // Manejo de excepciones al obtener el país.
            System.Diagnostics.Debug.WriteLine($"Error al obtener la ubicación: {ex.Message}");
            return "Error al obtener la ubicación";
        }
    }
}