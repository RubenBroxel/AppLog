
using LogApp.Services.ServicesManager.Models;

public interface IManager
{
    Task<ModelAccountService> MicroServiceAuthAccountAsync(ModelCredentialService userCredentials );
    Task<string> MicroServiceSendLogAsync(ModelAccountService userCredentials );
}