using Microsoft.Maui.Storage;
using System.Text;


 /*
    Descripcion:
    Clase para activar el envio de archivos al MicroServicio para Logs
    Secuencia de ejecución:
    [1]:Ejecutar el PrepareUploadFile(); :: { Método Privado }
    [2]:Ejecutar el PickToSendAsync();   :: { Método Privado }
    [3]:Ejecutar el UploadFileAsync(LogMicroService); { Método Publico }
*/
public class LocalServices: IFileServices
{
    private readonly HttpClient _httpClient;
    private const string FileNameKey = "MySecureFile";

    public LocalServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task UploadFileAsync(Stream fileStream, string filename, string token)
    {

    }
}