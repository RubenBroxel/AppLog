
namespace LogApp.Services.FileSystem.Contracts;
public interface IFileServices
{
      Stream? SearchLogFile(string path);
}