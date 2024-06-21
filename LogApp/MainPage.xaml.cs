using LogApp.Services;
using Microsoft.Extensions.Logging;


namespace LogApp;

public partial class MainPage : ContentPage
{
	private readonly IServiceProvider _serviceProvider;

	public MainPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		_serviceProvider = serviceProvider;
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		var commentPage = _serviceProvider.GetRequiredService<CommentPage>();
        await Navigation.PushAsync(commentPage);
	}
	
}

