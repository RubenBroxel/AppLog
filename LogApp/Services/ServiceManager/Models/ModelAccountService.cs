namespace LogApp.Services.ServicesManager.Models;
public class ModelAccountService
{
    #region Private Fields

    private readonly string PathService = "api";
    private readonly string LogSessionService = "auth/token";

    #endregion

	public string?   AppService  {get;set;}
	public string?   AppName      {get; set;} 
	public string?   AppPackage   {get; set;} 
	public string?   AppVersion   {get; set;} 
	public string?   AppBuild     {get; set;} 
	public string?   AppIpAddress {get; set;} 
	public string?   AppCountry   {get; set;}
	public Location? AppLocation  {get; set;} 
	public string?   AppToken { get; set; }

	public ModelAccountService( string token )
	{
		AppService = "Fintech.Logger.Services.Account";
		AppName    = AppInfo.Current.Name;
		AppPackage = AppInfo.Current.PackageName;
		AppVersion = AppInfo.Current.VersionString;
		AppBuild   = AppInfo.Current.BuildString;
		AppToken   = token;
	}

	public string GetLogMicroService()
    {
        return $"{PathService}/{LogSessionService}";
    }

    /// <summary>
    /// Gets the user's authentication token.
    /// </summary>
    /// <returns>The user's authentication token.</returns>
    public string GetTokenValid()
    {
        return AppToken ?? string.Empty;
    }

}
