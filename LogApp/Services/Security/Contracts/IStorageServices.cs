using System.Threading.Tasks;

namespace LogApp.Services.Security.Contracts;

public interface IStorageService
{
    Task<bool> SetSecurePathAsync(string key, string path);
    Task<string> GetSecurePathAsync(string key);
    Task<bool> RemoveSecurePathAsync(string key);
}
