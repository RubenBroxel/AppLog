using System.Net.Http.Headers;
using LogApp.Services.ServicesManager.Models;

/*
   Descripcion:
   Clase para activar el envio de archivos al MicroServicio para Logs
   Secuencia de ejecución:
   [1]:Ejecutar el PrepareUploadFile(); :: { Método Privado }
   [2]:Ejecutar el PickToSendAsync();   :: { Método Privado }
   [3]:Ejecutar el UploadFileAsync(LogMicroService); { Método Publico }
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
        var customFileTypes = new  FilePickerFileType (new Dictionary<DevicePlatform, IEnumerable<string>> 
        {
            { DevicePlatform.Android, new[] {"*/*.log", "*/*.Log", "*/*.LOG"} }
        });
        PickOptions options = new PickOptions();

        try
		{   
            /*
            var filePicker = await FilePicker.Default.PickAsync();//options);
            if(filePicker != null )
            {*/
            if( logMicroService.InvokeLogObject() != null)
            {

                using var stream = logMicroService.InvokeLogObject();//await filePicker.OpenReadAsync();
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
                    #if DEBUG
			        Console.WriteLine("Algo paso");
                    #endif
                }
            }
		}
		catch(HttpRequestException ex)
		{
            result = UPLOAD_SERVICE_FAILD;
			#if DEBUG
			    Console.WriteLine( ex.Message.ToString());
            #endif
		}
        
        return result;
    }
}