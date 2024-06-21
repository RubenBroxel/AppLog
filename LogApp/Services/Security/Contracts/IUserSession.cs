using LogApp.Services.ServicesManager.Models;
namespace LogApp.Services.Security.Contracts;

public interface IUserSessionServices
{
    Task<string> GetTokenServiceAsync(ModelAccountService userCredential);
}