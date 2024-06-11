using LogApp.Services;
using Microsoft.Extensions.Logging;


namespace LogApp;

public partial class MainPage : ContentPage
{
	ILogger<MainPage> _logger;

	private readonly IServiceProvider _serviceProvider;

	public MainPage(IServiceProvider serviceProvider ,ILogger<MainPage> logger, HttpClient httpClient )
	{
		InitializeComponent();
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		var commentPage = _serviceProvider.GetRequiredService<CommentPage>();
        await Navigation.PushAsync(commentPage);
	}
	
}

