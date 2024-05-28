


namespace LogApp.Services.ServicesManager.Models;
public class LogMicroService
{
    //api
    private string? PATH_SERVICE { get; set; }
    //"auth/token
    private string? SESSION_ACCOUNT_SERVICE { get; set; }
    //logservice
    private string? LOGFILE_UPLOAD_SERVICE { get; set; }



    public string InvokeTokenService()
    {
        return PATH_SERVICE + "/"+ LOGFILE_UPLOAD_SERVICE;
    }

    public string InvokeLogService()
    {
        return PATH_SERVICE + "/" + SESSION_ACCOUNT_SERVICE;
    }  
}