using LogApp.Services.ServicesManager.Models;
using Microsoft.Extensions.Logging;

namespace LogApp.Services;

/// <summary>
/// Vista para enviar un comentario.
/// </summary>
public partial class CommentPage : ContentPage
{
    private readonly IManager _manager;
    private readonly ILogger<CommentPage> _logger;

    /// <summary>
    /// Constructor de la clase CommentPage.
    /// </summary>
    /// <param name="manager">Instancia de la clase IManager para la gestión de servicios.</param>
    /// <param name="logger">Instancia de la clase ILogger para el registro de información.</param>
    public CommentPage(IManager manager, ILogger<CommentPage> logger)
    {
        InitializeComponent(); 
        _manager = manager;
        _logger = logger;
    }

 /// <summary>
    /// Maneja el evento click del botón de enviar comentario.
    /// </summary>
    /// <param name="sender">El objeto que originó el evento.</param>
    /// <param name="e">Argumentos del evento.</param>
    private async void OnCounterClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(CommentEdt.Text))
        {
            await DisplayAlert("Aviso", "Escriba un comentario para envío", "Ok");
            return; 
        }

        try
        {
            string user = "John Tobbias";
            string pass = "1234";
            _logger.LogInformation("{User} - {Comment}", user, CommentEdt.Text);

            ModelCredentialService userCredentials = new ModelCredentialService(user, pass);
            
            var credential = await _manager.MicroServiceAuthAccountAsync(userCredentials);
            if (credential != null)
            {
                var logResponse = await _manager.MicroServiceSendLogAsync(credential);
                await DisplayAlert("Aviso", logResponse ?? "Comentario enviado", "Ok");
            }
            else
            {
                await DisplayAlert("Aviso", "Error de autenticación", "Ok");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar comentario.");
            await DisplayAlert("Error", "Ocurrió un error al enviar el comentario.", "Ok");
        }
    }

    /// <summary>
    /// Maneja el evento click del botón para cerrar la página.
    /// </summary>
    /// <param name="sender">Objeto que desencadenó el evento.</param>
    /// <param name="e">Argumentos del evento.</param>
    private async void OnCloseCommentPage(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}