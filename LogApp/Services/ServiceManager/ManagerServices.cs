using LogApp.Services.FileSystem.Contracts;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;
using System.Diagnostics;

namespace LogApp.Services.ServicesManager;

public class Manager : IManager
{
    private readonly IUserSessionServices _userSessionServices;
    private readonly IMicroServices _microServices;
    private readonly IFileServices _fileServices;
    private readonly IStorageServices _storageServices;
    private readonly IAccountMicroService _accountMicroService;
    private readonly string LOG_MICRO_SERVICE_FAILD = "Servicio Fallo";

    public Manager
    (
        IUserSessionServices userSessionServices, 
        IMicroServices microServices, 
        IFileServices fileServices, 
        IStorageServices storageService, 
        IAccountMicroService accountMicroService
    )
    {
        _userSessionServices = userSessionServices;
        _microServices = microServices;
        _fileServices = fileServices;
        _storageServices = storageService;
        _accountMicroService = accountMicroService;
    }

    public async Task<ModelAccountService?> MicroServiceAuthAccountAsync(ModelCredentialService userCredentials)
    {
        try
        {
            //await _storageServices.SaveSecurePathAsync();
            var response = await _accountMicroService.GetTokenServiceAsync(userCredentials);
            
            // Manejar el caso donde response pueda ser nulo o vacío.
            if (response == null)
            {
                // Registrar el error (con más detalles si es posible).
                Debug.WriteLine("Error: GetTokenServiceAsync devolvió un valor nulo.");
                return null; 
            }
            
            var credencial = await _accountMicroService.CreateServiceAccountAsync(response);
            return credencial;

        }
        catch (Exception ex)
        {
            // Registrar la excepción con más detalles
            Debug.WriteLine($"{LOG_MICRO_SERVICE_FAILD} -  {ex.Message} \n StackTrace: {ex.StackTrace}");
            // Manejar la excepción, por ejemplo, retornando un valor por defecto o un mensaje de error.
            return null;
        }
    }

    public async Task<string> MicroServiceSendLogAsync(ModelAccountService userCredentials)
    {
        try
        {
            if (!string.IsNullOrEmpty(userCredentials?.AppToken)) // Verificar si userCredentials y appToken no son nulos.
            {
                await _storageServices.SaveSecurePathAsync();
                var token = await _userSessionServices.GetTokenServiceAsync(userCredentials);
                var path = await _storageServices.GetSecurePathAsync();

                // Manejar la posibilidad de que SearchLogFile devuelva nulo.
                var logFileStream = _fileServices.SearchLogFile(path); 
                if (logFileStream == null) 
                {
                    Debug.WriteLine("Error: No se encontró el archivo de registro.");
                    return LOG_MICRO_SERVICE_FAILD;
                }

                using (logFileStream) // Asegurarse de que el Stream se cierre.
                {
                    ModelMicroService logModelService = new ModelMicroService(logFileStream, token);
                    var responseService = await _microServices.MicroServicesAsync(logModelService);

                    if (responseService != null)
                    {
                        await _storageServices.RemoveSecureAsync();
                        return responseService;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Registrar la excepción con más detalles
            Debug.WriteLine($"{LOG_MICRO_SERVICE_FAILD} -  {ex.Message} \n StackTrace: {ex.StackTrace}");
        }
        return LOG_MICRO_SERVICE_FAILD;
    }
}