
using LogApp.Services.ServicesManager.Models;

public interface IManager
{
    Task<AccountModelService> MicroServiceAuthAccountAsync(UserCredentials userCredentials );
    Task<string> MicroServiceSendLogAsync(AccountModelService userCredentials );
}