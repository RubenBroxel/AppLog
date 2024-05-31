using LogApp.Services.ServicesManager.Models;

public interface IJwtServices
{
    Task<string> GetTokenServiceAsync(UserCredentials userCredentials, LogMicroService logMicroService);
}