

namespace LogApp.Services.ServicesManager.Models;
public class AccountModelService
{

	public string appName      {get; set;} //= AppInfo.Current.Name;
	public string appPackage   {get; set;} //= AppInfo.Current.PackageName;
	public string appVersion   {get; set;} //= AppInfo.Current.VersionString;
	public string appBuild     {get; set;} //= AppInfo.Current.BuildString;
	public string appIpAddress {get; set;} 
	public string appCountry   {get; set;}
	public Location? appLocation  {get; set;} 
	public string appToken { get; set; }

	public AccountModelService( string token )
	{
		appName    = AppInfo.Current.Name;
		appPackage = AppInfo.Current.PackageName;
		appVersion = AppInfo.Current.VersionString;
		appBuild   = AppInfo.Current.BuildString;
		appToken   = token;
	}
	
	/*string ipAddress = await GetPublicIP();
	var location = await GetCountryFromLocationAsync();//GetCountryAsync();//GetLocationAsync() ?? null ;*/	
}