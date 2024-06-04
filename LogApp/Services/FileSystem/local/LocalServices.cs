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
    private const string FileNameKey = "MySecureFile";

    public Stream? SearchLogFile(string path)
    {
        // Obtener la ruta de la carpeta principal de la aplicación
        //string carpetaPrincipal =FileSystem.Current.AppDataDirectory;
        string carpetaPrincipal = path;//"/storage/emulated/0/Android/data/com.demotechnical.logapp/DemoTechnical/";

        // Crear el patrón de búsqueda para el archivo log
        string fechaActual = DateTime.Now.ToString("yyyyMMdd");
        string nombreArchivo = $"Log - {fechaActual}.log";
        string patronBusqueda = $"{nombreArchivo}";

        // Buscar el archivo log en la carpeta principal
        string[] archivosEncontrados = Directory.GetFiles(carpetaPrincipal, patronBusqueda);

        // Verificar si se encontró algún archivo
        if (archivosEncontrados.Length > 0)
        {
            // Devolver el primer archivo encontrado como un Stream
            string rutaArchivo = archivosEncontrados[0];
            return File.OpenRead(rutaArchivo);
        }
        else
        {
            // No se encontró ningún archivo log
            return null;
        }
    }
}