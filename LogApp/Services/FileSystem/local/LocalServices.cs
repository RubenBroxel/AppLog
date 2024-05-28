using Microsoft.Maui.Storage;
using System.Text;

public class SecureStorageService: IFileServices
{
    private const string FileNameKey = "MySecureFile";

    public async Task SaveFileSecurelyAsync(string storageKey, string storageSecret)
    {
        try
        {
            await SecureStorage.SetAsync("MyKey", "MySecretValue");
        }
        catch (Exception ex)
        {
            // Posible error al guardar en SecureStorage
            Console.WriteLine($"Error al guardar en SecureStorage: {ex.Message}");
        }
    }

    public async Task GetFileSecureAsync(string storageKey)
    {
        try
        {
        string myValue = await SecureStorage.GetAsync("MyKey");
        if (!string.IsNullOrEmpty(myValue))
        {
            // Usar el valor recuperado
            Console.WriteLine($"Valor recuperado: {myValue}");
        }
        else
        {
            // La clave no existe o no se encontró ningún valor.
            Console.WriteLine("No se encontró la clave en SecureStorage.");
        }
        }
        catch (Exception ex)
        {
            // Posible error al obtener datos de SecureStorage
            Console.WriteLine($"Error al obtener datos de SecureStorage: {ex.Message}");
        }
    }

    public async Task DeleteFileSecureAsync()
    {
         SecureStorage.RemoveAll();
    }

}