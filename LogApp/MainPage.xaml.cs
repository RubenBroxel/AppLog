using LogApp.Services.ServicesManager;
using LogApp.Services.ServicesManager.Models;

using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LogApp.Services;

namespace LogApp;

public partial class MainPage : ContentPage
{
	int count = 0;
	ILogger<MainPage> _logger;
	private readonly HttpClient _httpClient;

	public MainPage( ILogger<MainPage> logger, HttpClient httpClient )
	{
		InitializeComponent();
		_logger = logger;
		_httpClient = httpClient;
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);

		await Navigation.PushAsync(new CommentPage());
		
	}

	/// <summary>
	/// Enviar a microservicio con la responsabilidad de enviar documentos a bucket en GCP
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public async void OnSendGcpClicked(object sender, EventArgs e)
	{
		bool respuesta = await DisplayAlert("Alerta", "¿Esta usted de acuerdo en enviar sus actividades a Broxel?", "Acepto", "Cancelar");
		if(respuesta)
		{
			var customFileTypes = new  FilePickerFileType (new Dictionary<DevicePlatform, IEnumerable<string>> 
			{
				{ DevicePlatform.Android, new[] {"*/*.log", "*/*.Log", "*/*.LOG"} }
			});

			string token = await GetToken();
			if (String.IsNullOrEmpty(token))
			{
				await DisplayAlert("Mensaje","Upps!, algo ocurrio...","ok");
			}
			else
			{				
				PickOptions options = new PickOptions();
				await PickToSend(options, token);
			}
		}
	}

	/// <summary>
	/// Método para enviar archivo log a servidor
	/// </summary>
	/// <param name="options"></param>
	/// <returns></returns>
	public async Task<FileResult> PickToSend(PickOptions options, string token)
	{
		try
		{
			var result = await FilePicker.Default.PickAsync(options);
			if(result != null )
			{
				using var stream = await result.OpenReadAsync();
				var broxelLog = ImageSource.FromStream(() => stream);
				await UploadFileAsync(stream, result.FileName, token);
			}
			//return result;
		}catch (Exception ex)
		{
			ex.Message.ToString();
		}
		return null;
	}

	/// <summary>
	/// función para enviar a servidor
	/// </summary>
	/// <param name="fileStream"></param>
	/// <param name="filename"></param>
	/// <returns></returns>
	async Task UploadFileAsync(Stream fileStream, string filename, string token)
	{
		try
		{
			var content = new MultipartFormDataContent();
			content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
			using var contenido = new StreamContent(fileStream, (int)fileStream.Length);
			Console.WriteLine(filename + "en upload" );

			content.Add(contenido,"file",filename);
			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
			using var response = await _httpClient.PostAsync("api/logservice/",content);
			if(response.IsSuccessStatusCode)
			{
				var file = await response.Content.ReadAsStringAsync();
				System.Diagnostics.Debug.WriteLine(file);
				await DisplayAlert("Mensaje","registro de actividades enviado","ok");
			}
		}
		catch(HttpRequestException ex)
		{
			await DisplayAlert("Mensaje","Upps! algo ocurrio durante el envio","ok");
			Console.WriteLine( ex.Message.ToString() );
		}

	}

	// línea para tomar la ruta de archivo 
	//var files = Directory.GetFiles(<FolderPath>)
	public async Task<string> GetToken()
	{
		string? TokenLog=null;
		try
		{
			string  usuario = "{ 'Username' : 'John Tobbias' , 'Password': '1234' }";
			_httpClient.DefaultRequestHeaders.Clear();
			using dynamic jsonString = JObject.Parse(usuario);
			using var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8,"application/json");
			
			using var response =  await _httpClient.PostAsync("api/auth/token",httpContent);
			if(response.IsSuccessStatusCode)
			{
				var jsonresponse = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<JsonLogToken>(jsonresponse);
				TokenLog = result?.TokenLog;
			}
		
		}catch(HttpRequestException ex)
		{
			await DisplayAlert("Mensaje","Upps! algo ocurrio durante el envio","ok");
			Console.WriteLine(ex.Message.ToString());
		}
		return TokenLog;
	}
	


}

