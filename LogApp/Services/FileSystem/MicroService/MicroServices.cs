using System.Net.Http.Headers;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.FileSystem.MicroService;

public class MicroService : IMicroServices
{
    private readonly string UploadServiceSuccess = "Registro enviado con exito";
    private readonly string UploadServiceFailed = "Upps!... algo ocurrio durante el envio, favor de volver a intentar.";

    private readonly HttpClient _httpClient;
    private readonly string _urlServiceAccount = "http://10.100.8.2:8484/";

    public MicroService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_urlServiceAccount);
    }

    public async Task<string> MicroServicesAsync(ModelMicroService logMicroService)
    {
        try
        {
            using var stream = logMicroService.GetFileStream();
            if (stream == null)
            {
                return UploadServiceFailed; 
            }

            using var content = new MultipartFormDataContent
            {
                { new StreamContent(stream, (int)stream.Length), logMicroService.GetSupportType(), logMicroService.GetFileName() }
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(logMicroService.GetTokenValid());

            using var response = await _httpClient.PostAsync(logMicroService.GetLogMicroService(), content);
            
            if (response.IsSuccessStatusCode)
            {
                // Evitando el warning de consola utilizando la variable 'file' solo si DEBUG está activo
                #if DEBUG
                var file = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(file); 
                #endif

                return UploadServiceSuccess;
            }
            
            System.Diagnostics.Debug.WriteLine(UploadServiceFailed); // Usando Debug.WriteLine en lugar de Console.WriteLine
            return UploadServiceFailed;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString()); // Usando Debug.WriteLine en lugar de Console.WriteLine
            return UploadServiceFailed; 
        }
    }
}