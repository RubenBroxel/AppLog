
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LogApp.Services.ServicesManager.Models;

public class LogLocal
{
    private const int KeySize = 2048;

    private string? LogAccess = Guid.NewGuid().ToString() + "-" + DateTime.Today.ToString();

    public string GetKey()
    {
        return LogAccess;
    }

    public static void GenerateKeys(out string publicKey, out string privateKey)
    {
        using var rsa = RSA.Create(KeySize);
        publicKey  = rsa.ToXmlString(false);
        privateKey = rsa.ToXmlString(true);
    }


    public static byte[] EncryptData(byte[] dataToEncrypt, string publicKeyXml)
    {
        using var rsa = RSA.Create();
        rsa.FromXmlString(publicKeyXml);
        return rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
    }


    public static byte[] DecryptData(byte[] dataToDecrypt, string privateKeyXml)
    {
        using var rsa = RSA.Create();
        rsa.FromXmlString(privateKeyXml);

        return rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
    }

    /* Genera claves RSA
    string publicKey, privateKey;
    RSAEncryption.GenerateKeys(out publicKey, out privateKey);

    // Ruta al archivo
    string filePath = Path.Combine(FileSystem.AppDataDirectory, "myfile.txt");

    // Leer datos del archivo
    byte[] data = File.ReadAllBytes(filePath);

    // Cifrar datos
    byte[] encryptedData = RSAEncryption.EncryptData(data, publicKey);

    // Guardar datos cifrados
    File.WriteAllBytes(filePath + ".enc", encryptedData);

    // Descifrar datos
    byte[] decryptedData = RSAEncryption.DecryptData(encryptedData, privateKey);

    // Guardar datos descifrados
    File.WriteAllBytes(filePath + ".dec", decryptedData);*/
}