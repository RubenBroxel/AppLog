using System.Security.Cryptography;
using LogApp.Services.Security.Contracts;

namespace LogApp.Services.Security.Cypher;
public class Aes256Service:ICypherServices
{
    private const int KeySize = 256;
    private const int BlockSize = 128;
    private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.CBC;
    private const PaddingMode PaddingMode = System.Security.Cryptography.PaddingMode.PKCS7;

    // Genera una nueva clave y vector de inicialización
    public Tuple<string, string> GenerateKeyAndIV()
    {
        using var aes = Aes.Create();
        try
        {
            
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.GenerateKey();
            aes.GenerateIV();
            return Tuple.Create(Convert.ToBase64String(aes.Key), Convert.ToBase64String(aes.IV));
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        return null;
    }

    // Cifra un archivo
    public  void EncryptFile(string inputFile, string outputFile, string key, string iv)
    {
        try
        {
            // Convierte la clave y el vector de inicialización de base64 a byte[]
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] ivBytes = Convert.FromBase64String(iv);

            // Crea un Aes object con la configuración especificada
            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode;
            aes.Padding = PaddingMode;

            // Crea los objetos necesarios para el cifrado
            using var encryptor = aes.CreateEncryptor(keyBytes, ivBytes);
            using var fileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            using var cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Read);
            using var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

            // Cifra el archivo
            cryptoStream.CopyTo(outputStream);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

    }

    // Descifra un archivo
    public void DecryptFile(string inputFile, string outputFile, string key, string iv)
    {
        try
        {
            // Convierte la clave y el vector de inicialización de base64 a byte[]
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] ivBytes = Convert.FromBase64String(iv);

            // Crea un Aes object con la configuración especificada
            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode;
            aes.Padding = PaddingMode;

            // Crea los objetos necesarios para el descifrado
            using var decryptor = aes.CreateDecryptor(keyBytes, ivBytes);
            using var fileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            using var cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Read);
            using var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

            // Descifra el archivo
            cryptoStream.CopyTo(outputStream);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

    }
}