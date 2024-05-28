
namespace LogApp.Services;

public partial class CommentPage : ContentPage
{

	public CommentPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		await DisplayAlert("Envio","Comentarios enviados","Ok");
	}	


	private async void OnCloseCommentPage(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}	
}

