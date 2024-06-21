namespace LogApp.Services.ServicesManager.Models;
public class ModelCredentialService
{
    #region Private Fields

    private readonly string PathService = "api";
    private readonly string SessionService = "auth/user";

    #endregion

    public string? UserName { get; set; }
    public string? UserPassword { get; set; }


    public ModelCredentialService(string userName, string userPassword)
    {
        UserName = userName;
        UserPassword = userPassword;
    }
    

    /// <summary>
    /// Gets the endpoint for uploading the log file.
    /// </summary>
    /// <returns>The endpoint for uploading the log file.</returns>
    public string GetLogMicroService()
    {
        return $"{PathService}/{SessionService}";
    }
}