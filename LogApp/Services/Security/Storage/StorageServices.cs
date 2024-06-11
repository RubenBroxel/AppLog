using LogApp.Services.Security.Contracts;

namespace LogApp.Services.Security.Storage;
public class StorageService: IStorageServices
{
    private readonly string SecureStorageKey = Guid.NewGuid().ToString();

    /// <summary>
    /// Guarda una ruta de forma segura utilizando SecureStorage.
    /// </summary>
    /// <returns>Un valor booleano que indica si la operación se realizó correctamente.</returns>
    public async Task<bool> SaveSecurePathAsync()
    {
        try
        {
            #if ANDROID
                string? PATH_LOG = Android.App.Application.Context?.GetExternalFilesDir("")?.AbsolutePath.ToString();

            #elif IOS
                string? PATH_LOG = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Library/Caches");
            #endif

            await SecureStorage.SetAsync(SecureStorageKey, PATH_LOG);
            return true;
        }
        catch (Exception ex)
        {
            // Manejar la excepción, por ejemplo, registrar el error
            Console.WriteLine($"Error al guardar la ruta: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Obtiene la ruta guardada de forma segura desde SecureStorage.
    /// </summary>
    /// <returns>La ruta guardada o una cadena vacía si no se encontró ninguna ruta.</returns>
    public async Task<string> GetSecurePathAsync()
    {
        try
        {
            return await SecureStorage.GetAsync(SecureStorageKey) ?? string.Empty;
        }
        catch (Exception ex)
        {
            // Manejar la excepción, por ejemplo, registrar el error
            Console.WriteLine($"Error al obtener la ruta: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task RemoveSecureAsync()
    {
        SecureStorage.RemoveAll();
    }
}