using LogApp.Services.ServicesManager.Models;
namespace LogApp.Services.Security.Contracts;

public interface IAccountMicroService
{
    Task<string> GetTokenServiceAsync(UserCredentials userCredential);
    Task<AccountModelService> CreateServiceAccountAsync(string token);
}