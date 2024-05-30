
namespace LogApp.Services.ServicesManager.Models;
public class LogMicroService
{
    //
    private Stream? FileStream;
    private string? FileName;
    private string? TokenValid;

    #region Variables privadas
    private readonly string? PATH_SERVICE = "api";
    private readonly string? TYPE_SUPPORT = "file";
    private readonly string? SESSION_ACCOUNT_SERVICE = "auth/token"; 
    private readonly string? LOGFILE_UPLOAD_SERVICE = "logservice";
    #endregion

    public LogMicroService(Stream fileStream, string fileName, string tokenUser)
    {
        FileStream = fileStream;
        FileName   = fileName;
        TokenValid = tokenUser;
    }


    public Stream InvokeLogObject()
    {
        return FileStream;
    } 

    public string InvokeLogObjectName()
    {
        return FileName;
    }

    public string InvokeLogObjectValidator()
    {
        return TokenValid;
    }

    //obtenemos el end-point para subir archivo 
    public string InvokeLogService()
    {
        return PATH_SERVICE + "/" + LOGFILE_UPLOAD_SERVICE;
    }

    //obtenemos el token
    public string InvokeTokenService()
    {
        return PATH_SERVICE + "/" + SESSION_ACCOUNT_SERVICE;
    }  

    public string InvokeSuport()
    {
        return TYPE_SUPPORT;
    }
}