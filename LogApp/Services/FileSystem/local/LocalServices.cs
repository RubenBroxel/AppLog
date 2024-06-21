using LogApp.Services.FileSystem.Contracts;
using LogApp.Services.Security.Contracts;

namespace LogApp.Services.FileSystem.Local;
 /*
    Descripcion:
    Clase para activar el envio de archivos al MicroServicio para Logs
*/
public class LocalService: IFileServices
{
    public Stream? SearchLogFile(string path)
    {
        string carpetaPrincipal = path;
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