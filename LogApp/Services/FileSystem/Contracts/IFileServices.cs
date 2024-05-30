public interface IFileServices
{
     Task UploadFileAsync(Stream fileStream, string filename, string token);
}