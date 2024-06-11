using LogApp.Services.ServicesManager.Models;
using Microsoft.Extensions.Logging;

namespace LogApp.Services;

public partial class CommentPage : ContentPage
{
	private IManager _manager;
	ILogger<CommentPage> _logger;
	public CommentPage(IManager manager, ILogger<CommentPage> logger)
	{
		InitializeComponent();
		_manager = manager;
		_logger = logger;
	}

   	private async void OnCounterClicked(object sender, EventArgs e)
	{	
		if (!string.IsNullOrEmpty(CommentEdt.Text))
		{
			string user = "John Tobbias";
			string pass = "1234";
			_logger.LogInformation(user + " - " +CommentEdt.Text);
			UserCredentials userCredentials = new UserCredentials(user, pass);
			var credencial = await _manager.MicroServiceAuthAccountAsync(userCredentials);
			//var mensaje    = await _manager?.MicroServiceAuthAccountAsync(credencial); 
			//await DisplayAlert("Aviso", mensaje, "Ok"); 
		}
		else
		{
			// Si ClosePageBtn.Text no está vacío, muestra el aviso
			await DisplayAlert("Aviso", "Escriba un comentario para envío", "Ok"); 
		}
	}	


	private async void OnCloseCommentPage(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}	
}

