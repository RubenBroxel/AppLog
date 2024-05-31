using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using LogApp.Services.Security.Contracts;


namespace LogApp.Services.Security.Storage;

public class StorageService : IStorageService
{
    public async Task<bool> SetSecurePathAsync(string key, string path)
    {
        try
        {
            await SecureStorage.SetAsync(key, path);
            return true;
        }
        catch (System.Exception)
        {
            // Manejo de excepciones (log, etc.)
            return false;
        }
    }

    public async Task<string> GetSecurePathAsync(string key)
    {
        try
        {
            return await SecureStorage.GetAsync(key);
        }
        catch (System.Exception)
        {
            // Manejo de excepciones (log, etc.)
            return null;
        }
    }

    public async Task<bool> RemoveSecurePathAsync(string key)
    {
        try
        {
            SecureStorage.RemoveAll();
            return true;
        }
        catch (System.Exception)
        {
            // Manejo de excepciones (log, etc.)
            return false;
        }
    }
}
