using System.Threading.Tasks;

namespace LogApp.Services.Security.Contracts;

public interface IStorageServices
{
    Task<bool> SaveSecurePathAsync();
    Task<string> GetSecurePathAsync();
    Task RemoveSecureAsync();
}
