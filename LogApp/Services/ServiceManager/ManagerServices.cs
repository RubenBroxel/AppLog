using LogApp.Services.ServicesManager.Models;

namespace LogApp.Services.ServicesManager
{

	public class Manager : IManager
	{
		private readonly HttpClient _httpClient;

	

		public Manager(HttpClient httpClient)
		{
			_httpClient = httpClient;

		}

		public async void PrepareUploadFile()
		{
			//bool respuesta = await Page.DisplayAlert("Alerta", "¿Esta usted de acuerdo en enviar sus actividades a Broxel?", "Acepto", "Cancelar");
			bool respuesta = false;
			if(respuesta)
			{
				var customFileTypes = new  FilePickerFileType (new Dictionary<DevicePlatform, IEnumerable<string>> 
				{
					{ DevicePlatform.Android, new[] {"*/*.log", "*/*.Log", "*/*.LOG"} }
				});

				PickOptions options = new PickOptions();
				//ToDo: Ejecutar la subida de archivos
				//await FileMessageManagerAsync(options);
			}
		}


		public async void FileMessageManagerAsync(PickOptions options)
		{
			try
			{
				var result = await FilePicker.Default.PickAsync(options);
				if(result != null )
				{
					using var stream = await result.OpenReadAsync();
					var broxelLog = ImageSource.FromStream(() => stream);
					//ToDo: Mensaje de Aprobado
					//await UploadFileAsync(stream, result.FileName);
				}
				//return result;
			}catch 
			{
				//ex.Message.ToString();
			}
		}

		//Usar Microservicio
		public async void UserMicroServiceAuthAsync(Stream fileStream, string filename)
		{
			try
			{
				using var content = new MultipartFormDataContent();
				content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
				using var contenido = new StreamContent(fileStream, (int)fileStream.Length);
				content.Add(contenido,"file",filename);
				using var response = await _httpClient.PostAsync("",content);
				if(response.IsSuccessStatusCode)
				{
					var file = await response.Content.ReadAsStringAsync();
					System.Diagnostics.Debug.WriteLine(file);
					//Todo: Devolver Mensaje de exito
				}
			}
			catch(HttpRequestException ex)
			{
				//ToDo: Mensaje de excepcion
				//await DisplayAlert("Mensaje","Upps! algo ocurrio durante el envio","ok");
				//Console.WriteLine( ex.Message.ToString() );
			}
		}

		//TODO: Obtener Token del Microservicio para subir archivo con ello se puede 
		private async Task<string> GetTokenMicroService()
		{
			
			return "";
		}
		
	}
}