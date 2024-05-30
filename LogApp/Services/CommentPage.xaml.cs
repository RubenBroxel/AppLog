
using LogApp.Services.ServicesManager;
using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services;

public partial class CommentPage : ContentPage
{
	private IManager _manager;
	public CommentPage()
	{
		InitializeComponent();
		_manager = DependencyService.Get<IManager>();
	}

   	private async void OnCounterClicked(object sender, EventArgs e)
	{
		_manager.MicroServiceAuthAsync();
		//string pasword = "";

		//await Xamarin.Essentials.SecureStorage.SetAsync("Password",SendCommentBtn.Text);
		try
		{
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

