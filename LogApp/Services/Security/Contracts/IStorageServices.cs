using System.Threading.Tasks;

namespace LogApp.Services.Security.Contracts;

public interface IStorageService
{
    Task<bool> SaveSecurePathAsync();
    Task<string> GetSecurePathAsync();
    Task RemoveSecureAsync();
}
