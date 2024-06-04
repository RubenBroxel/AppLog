
using Android.Util;
using LogApp.Services.ServicesManager;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services;

public partial class CommentPage : ContentPage
{
	private IManager _manager;
	IServiceProvider _provider;
	public CommentPage(IManager manager)
	{
		InitializeComponent();
		_manager = manager;
	}

   	private async void OnCounterClicked(object sender, EventArgs e)
	{
		string user = "John Tobbias";
		string pass = "1234";
		
		//string pasword = "";
		//await Xamarin.Essentials.SecureStorage.SetAsync("Password",SendCommentBtn.Text);
		try
		{
			UserCredentials userCredentials = new UserCredentials(user,pass);
			_manager.MicroServiceAuthAsync(userCredentials);
			//_logger.LogInformation(CounterBtn.Text , DateTime.UtcNow.ToLongTimeString());
			//pasword = await Xamarin.Essentials.SecureStorage.GetAsync("Password");
			//await DisplayAlert("Secure Success",pasword,"Ok");
			//_manager.MicroServiceAuthAsync( );
		}
		catch
		{
			await DisplayAlert("Secure Error","Aun no tiene","Ok");
		}
	}	


	private async void OnCloseCommentPage(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}	
}

