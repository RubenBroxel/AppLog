using LogApp.Services.FileSystem.Contracts;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.ServicesManager;

public class Manager : IManager
{
	private readonly IUserSessionServices _userSessionServices;
	private readonly IMicroServices _microServices;
	private readonly IFileServices _fileServices;
	private readonly IStorageServices _storageServices;
	private readonly IAccountMicroService _accountMicroService;
	
	public Manager
	(
		IUserSessionServices userSessionServices, IMicroServices microServices, IFileServices fileServices, 
		IStorageServices storageService, IAccountMicroService accountMicroService
	)
	{
		_userSessionServices = userSessionServices;
		_microServices       = microServices;
		_fileServices        = fileServices;
		_storageServices     = storageService;
		_accountMicroService = accountMicroService;
	}

    public async Task<AccountModelService> MicroServiceAuthAccountAsync(UserCredentials userCredentials)
    {
		try
		{
			await _storageServices.SaveSecurePathAsync();
			var response = await _accountMicroService.GetTokenServiceAsync(userCredentials);
			var credencial = await _accountMicroService.CreateServiceAccountAsync(response);
			return credencial;

		}catch(Exception ex)
		{
			return null;
		}
		return null;
    }

    public async Task<string> MicroServiceSendLogAsync(AccountModelService userCredentials)
    {
		await _storageServices.SaveSecurePathAsync();
        var token = await _userSessionServices.GetTokenServiceAsync(userCredentials);
		var path  = await _storageServices.GetSecurePathAsync();
		LogModelService logModelService = new LogModelService(_fileServices.SearchLogFile(path) ?? Stream.Null,"eee",token);
		var response = await _microServices.MicroServicesAsync(logModelService);
		if(response != null)
		{
			await _storageServices.RemoveSecureAsync();
		}
		
		return response ?? "";
    }
}
