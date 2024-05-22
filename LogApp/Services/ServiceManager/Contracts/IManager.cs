public interface IManager
{
    void FileMessageManagerAsync(PickOptions options);
    void UserMicroServiceAuthAsync(Stream fileStream, string filename);

    void PrepareUploadFile();
}