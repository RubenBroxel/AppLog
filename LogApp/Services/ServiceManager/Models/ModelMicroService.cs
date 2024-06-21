

namespace LogApp.Services.ServicesManager.Models;

/// <summary>
/// Represents a model for managing log file data.
/// </summary>
public class ModelMicroService
{
    #region Private Fields

    private readonly string PathService = "api";
    private readonly string TypeSupport = "file";
    private readonly string LogService = "logservice";

    #endregion

    #region Properties

    /// <summary>
    /// Gets the file stream for the log file.
    /// </summary>
    public Stream? FileStream { get; }

    /// <summary>
    /// Gets the name of the log file.
    /// </summary>
    public string? FileName { get; }

    /// <summary>
    /// Gets the user's authentication token.
    /// </summary>
    public string? TokenValid { get; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="LogModelService"/> class.
    /// </summary>
    /// <param name="fileStream">The file stream for the log file.</param>
    /// <param name="fileName">The name of the log file.</param>
    /// <param name="tokenUser">The user's authentication token.</param>
    public ModelMicroService(Stream fileStream, string tokenUserService)
    {
        FileStream = fileStream;
        FileName   = AppInfo.Current.Name + " - " + AppInfo.Current.Version + " - " + AppInfo.Current.BuildString ;
        TokenValid = tokenUserService;
    }

    /// <summary>
    /// Gets the file stream for the log file.
    /// </summary>
    /// <returns>The file stream for the log file.</returns>
    public Stream? GetFileStream()
    {
        return FileStream;
    }

    /// <summary>
    /// Gets the name of the log file.
    /// </summary>
    /// <returns>The name of the log file.</returns>
    public string GetFileName()
    {
        return FileName ?? string.Empty;
    }

    /// <summary>
    /// Gets the user's authentication token.
    /// </summary>
    /// <returns>The user's authentication token.</returns>
    public string GetTokenValid()
    {
        return TokenValid ?? string.Empty;
    }

    /// <summary>
    /// Gets the endpoint for uploading the log file.
    /// </summary>
    /// <returns>The endpoint for uploading the log file.</returns>
    public string GetLogMicroService()
    {
        return $"{PathService}/{LogService}";
    }

    /// <summary>
    /// Gets the type of support.
    /// </summary>
    /// <returns>The type of support.</returns>
    public string GetSupportType()
    {
        return TypeSupport;
    }
}