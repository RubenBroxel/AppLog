using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using LogApp.Services.Security.Contracts;



namespace LogApp.Services.Security.Storage;

public class StorageService: IStorageService
    {
        private readonly string SecureStorageKey = Guid.NewGuid().ToString();

        /// <summary>
        /// Guarda una ruta de forma segura utilizando SecureStorage.
        /// </summary>d53ae36d-90ed-45d8-b804-d6a6fc2a1063
        /// <param name="path">La ruta a guardar.</param>
        /// <returns>Un valor booleano que indica si la operación se realizó correctamente.</returns>
        public async Task<bool> SaveSecurePathAsync()
        {
            try
            {
                string path = "/storage/emulated/0/Android/data/com.demotechnical.logapp/DemoTechnical";
                //string path = FileSystem.Current.AppDataDirectory();
                await SecureStorage.SetAsync(SecureStorageKey, path);
                return true;
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrar el error
                Console.WriteLine($"Error al guardar la ruta: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene la ruta guardada de forma segura desde SecureStorage.
        /// </summary>
        /// <returns>La ruta guardada o una cadena vacía si no se encontró ninguna ruta.</returns>
        public async Task<string> GetSecurePathAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(SecureStorageKey) ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrar el error
                Console.WriteLine($"Error al obtener la ruta: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task RemoveSecureAsync()
        {
            SecureStorage.RemoveAll();
        }
    }