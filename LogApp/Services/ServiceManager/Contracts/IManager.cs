
using LogApp.Services.ServicesManager.Models;

public interface IManager
{
    Task<string> MicroServiceAuthAsync(UserCredentials userCredentials );
}