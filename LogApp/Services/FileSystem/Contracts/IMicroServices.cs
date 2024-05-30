using LogApp.Services.ServicesManager.Models;
public interface IMicroServices
{
     Task<string> MicroServicesAsync(LogMicroService logMicroService);
}