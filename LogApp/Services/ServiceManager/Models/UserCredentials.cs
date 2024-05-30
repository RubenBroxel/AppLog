namespace LogApp.Services.ServicesManager.Models;
public class UserCredentials
{
    private string? UserName { get; set; }
    private string? UserPassword { get; set; }


    public UserCredentials(string userName, string userPassword)
    {
        UserName = userName;
        UserPassword = userPassword;
    }
    
}