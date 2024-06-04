using System.Net.Http.Headers;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.FileSystem.MicroService;
/*
   Descripcion:
   Clase para activar el envio de archivos al MicroServicio para Logs
*/
public class MicroServices: IMicroServices
{
    private readonly HttpClient _httpClient;

    private readonly string UPLOAD_SERVICE_SUCCESS = "Registro enviado con exito";
    private readonly string UPLOAD_SERVICE_FAILD = "Upps!... algo ocurrio durante el envio, favor de volver a intentar.";

    public MicroServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> MicroServicesAsync(LogMicroService logMicroService)
	{
        string? result = null;
        try
		{   
            if( logMicroService.InvokeLogObject() != null)
            {

                using var stream = logMicroService.InvokeLogObject();
                var broxelLog = ImageSource.FromStream(() => stream);

                using var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
                using var contenido = new StreamContent(logMicroService.InvokeLogObject(), (int)logMicroService.InvokeLogObject().Length);
                content.Add(contenido,logMicroService.InvokeSuport(),logMicroService.InvokeLogObjectName());
                
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(logMicroService.InvokeLogObjectValidator());
                
                using HttpResponseMessage response = await _httpClient.PostAsync(logMicroService.InvokeLogService(),content);
                if(response.IsSuccessStatusCode)
                {
                    var file = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(file);
                    result = UPLOAD_SERVICE_SUCCESS;
                }
                else
                {
                    result = UPLOAD_SERVICE_FAILD;
                    #if DEBUG
			        Console.WriteLine(UPLOAD_SERVICE_FAILD);
                    #endif
                }
            }
		}
		catch(Exception ex)
		{
            result = UPLOAD_SERVICE_FAILD;
			#if DEBUG
			    Console.WriteLine( ex.Message.ToString());
            #endif
		}
        return result;
    }
}