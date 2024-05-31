
using LogApp.Services.Security.Contracts;
using LogApp.Services.Security.Cypher;
using LogApp.Services.ServicesManager.Models;
using System.Text;

namespace LogApp.Services.ServicesManager;

public class Manager : IManager
{
	private readonly IUserSessionServices _userSessionServices;
	private readonly IMicroServices _microServices;
	private readonly IFileServices _fileServices;
	public Manager(IUserSessionServices userSessionServices, IMicroServices microServices, IFileServices fileServices)
	{
		_userSessionServices = userSessionServices;
		_microServices = microServices;
		_fileServices  = fileServices;
	}

    public async Task<string> MicroServiceAuthAsync(UserCredentials userCredentials)
    {
		//ToDo: buscar el archivo log dentro del celular usando la ruta 
		//predeterminada y el nombre de la carpeta dela aplicacion
        var token = await _userSessionServices.GetTokenServiceAsync(userCredentials);
		var log = new RsaServices();

		var logKey = log.GetKey();
		// Encriptar datos
		var dataToEncrypt = Encoding.UTF8.GetBytes(logKey);
		var encryptedData = log.EncryptData(dataToEncrypt);
	
		LogMicroService logMicroService = new LogMicroService(_fileServices.SearchLogFile(),logKey,token);
		var response = await _microServices.MicroServicesAsync(logMicroService);
        //Console.WriteLine(response);

		// Desencriptar datos
		var decryptedData = log.DecryptData(encryptedData);
		var decryptedMessage = Encoding.UTF8.GetString(decryptedData);
		return response;
    }
}
