using LogApp.Services.Security.Contracts;
using LogApp.Services.Security.Cypher;
using LogApp.Services.ServicesManager.Models;
using MetroLog;
using System.Text;

namespace LogApp.Services.ServicesManager;

public class Manager : IManager
{
	private readonly IUserSessionServices _userSessionServices;
	private readonly IMicroServices _microServices;
	private readonly IFileServices _fileServices;
	private readonly IStorageService _storageServices;
	
	public Manager(IUserSessionServices userSessionServices, IMicroServices microServices, IFileServices fileServices, IStorageService storageService)
	{
		_userSessionServices = userSessionServices;
		_microServices = microServices;
		_fileServices  = fileServices;
		_storageServices = storageService;
	}

    public async Task<string> MicroServiceAuthAsync(UserCredentials userCredentials)
    {
		await _storageServices.SaveSecurePathAsync();
        var token = await _userSessionServices.GetTokenServiceAsync(userCredentials);
		var path  = await _storageServices.GetSecurePathAsync();
		LogMicroService logMicroService = new LogMicroService(_fileServices.SearchLogFile(path) ?? Stream.Null,"eee",token);
		var response = await _microServices.MicroServicesAsync(logMicroService);
		if(response != null)
		{
			await _storageServices.RemoveSecureAsync();
		}
		
		return response ?? "";
    }
}
