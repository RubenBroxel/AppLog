
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.ServicesManager;

/// <summary>
/// Descripcion:
///	  Clase para activar el envio de archivos al MicroServicio para Logs
///	  Secuencia de ejecución:
///	  [1]:Ejecutar Carga se sesión 
///	  [2]:Ejecutar Seguridad al cargar sesión
///   [2]:Ejecutar Preparación de subida del Archivo *.log con información
///	  [3]:Ejecutar Subir archivo a Micro Servicio 
///	  [4]:Ejecutar Eliminación de archivo seguro y terminar la sesión para Micro Servicio
/// </summary>
public class Manager : IManager
{
	//private readonly IMicroServices _microServices;
	private readonly IUserSessionServices _userSessionServices;

	public Manager()
	{
		//_microServices = microServices;
		_userSessionServices = DependencyService.Get<IUserSessionServices>();
	}

	/*
	e.StackTrace
	*/
	public async void MicroServiceAuthAsync()//UserCredentials userCredentials)
	{
		
		var response = await _userSessionServices.GetTokenServiceAsync();//userCredentials);
		Console.WriteLine(response);
		//_microServices.MicroServicesAsync(logMicroService);
	}
}