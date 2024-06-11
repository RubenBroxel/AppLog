
namespace LogApp.Services.Security.Contracts;
public interface ICypherServices
{
    Tuple<string, string> GenerateKeyAndIV();
    void EncryptFile(string inputFile, string outputFile, string key, string iv);
    void DecryptFile(string inputFile, string outputFile, string key, string iv);
}