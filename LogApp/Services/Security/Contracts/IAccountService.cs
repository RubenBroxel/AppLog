using LogApp.Services.ServicesManager.Models;
namespace LogApp.Services.Security.Contracts;

public interface IAccountMicroService
{
    Task<string> GetTokenServiceAsync(ModelCredentialService userCredential);
    Task<ModelAccountService> CreateServiceAccountAsync(string token);
}