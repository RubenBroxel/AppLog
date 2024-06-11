using System.Text;
using Newtonsoft.Json;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.Security;
public class LogMicroService: IUserSessionServices
{
    private readonly HttpClient _httpClient;
    public LogMicroService(HttpClient httpClient)
    {
        _httpClient = httpClient;


    }

    public async Task<string> GetTokenServiceAsync(AccountModelService userCredentials)
    {
        string? TokenLog = null;
        try
        {        
            var userJson = new 
			{ 
				UserAccount     = "", 
				TokenAccount    = "" , 
				AppMobile       = userCredentials.appName,
				AppPackage      = userCredentials.appPackage,
				AppBuild        = userCredentials.appBuild,
				AppVersion      = userCredentials.appVersion,
				CountryAccount  = userCredentials.appCountry,
				AddressAccount  = userCredentials.appIpAddress,
				LocationAccount = userCredentials.appLocation,
			};
            
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
                var result = JsonConvert.DeserializeObject<LogModelToken>(jsonresponse);
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


    private async Task<string> GetPublicIP()
	{
		try
		{
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync("https://api.ipify.org"); 
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error al obtener la IP pública: {ex.Message}");
			return null; 
		}
	}

	private async Task<Location?> GetLocationAsync()
	{
		#if ANDROID
		try
		{
			// Solicitar permisos al usuario si no se han concedido.
			var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
			var location = await Geolocation.Default.GetLocationAsync(request);

			if (location != null)
			{
				Console.WriteLine($"Latitud: {location.Latitude}, Longitud: {location.Longitude}, Altitud: {location.Altitude}");
				return location;
			}
			else
			{
				Console.WriteLine("No se pudo obtener la ubicación.");
				return null;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error al obtener la ubicación: {ex.Message}");
			return null;
		}

		#elif IOS
		try
		{
			var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
			var location = await Geolocation.GetLocationAsync(request);

			if (location != null)
			{
				Console.WriteLine($"Latitud: {location.Latitude}, Longitud: {location.Longitude}, Altitud: {location.Altitude}");
				return location;
			}
			return null;
		}
		catch (FeatureNotSupportedException fnsEx)
		{
			return null;
			// Manejar la excepción si el dispositivo no soporta la geolocalización
		}
		catch (FeatureNotEnabledException fneEx)
		{
			return null;
			// Manejar la excepción si la geolocalización no está habilitada en el dispositivo
		}
		catch (PermissionException pEx)
		{
			return null;
			// Manejar la excepción si no se tienen los permisos necesarios
		}
		catch (Exception ex)
		{
			return null;
			// Manejar cualquier otra excepción
		}
		// Manejar las mismas excepciones que en el ejemplo anterior
		#endif
	}
	#if IOS
	private async Task<string> GetCountryAsync()
	{
		try
		{
			var location = await Geolocation.GetLastKnownLocationAsync();

			if (location != null)
			{
				var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
				var placemark = placemarks?.FirstOrDefault();
				return placemark?.CountryCode; 
			}
		}
		catch (FeatureNotSupportedException)
		{
			// Manejar dispositivos que no soportan geolocalización
		}
		catch (PermissionException)
		{
			// Manejar falta de permisos
		}
		catch (Exception ex)
		{
			// Manejar otros errores
		}

		return null;
	}
	#endif

	private async Task<string> GetCountryFromLocationAsync()
	{
		try
		{
			var request = new GeolocationRequest(GeolocationAccuracy.Medium);
			var location = await Geolocation.GetLocationAsync(request);

			if (location != null)
			{
				// Obtener la dirección a partir de las coordenadas.
				var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
				var placemark = placemarks?.FirstOrDefault();

				// Devolver el país.
				return placemark?.CountryName ?? "País no encontrado";
			}
			else
			{
				return "No se pudo obtener la ubicación.";
			}
		}
		catch (Exception ex)
		{
			// Manejar excepciones (por ejemplo, permisos denegados).
			Console.WriteLine($"Error al obtener la ubicación: {ex.Message}");
			return "Error al obtener la ubicación";
		}
	}


}