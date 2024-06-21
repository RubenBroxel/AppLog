using MetroLog.Operators;
using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Logging;
using LogApp.Services;
using LogApp.Services.ServicesManager;
using LogApp.Services.Security;
using LogApp.Services.Security.Contracts;
using LogApp.Services.ServicesManager.Models;
using LogApp.Services.Security.Storage;
using LogApp.Services.FileSystem.Local;
using LogApp.Services.FileSystem.MicroService;
using LogApp.Services.Security.Cypher;
using LogApp.Services.FileSystem.Contracts;


namespace LogApp;

public static class MauiProgram
{
	// "/storage/emulated/0/Android/data/com.demotechnical.logapp/files"
	#if ANDROID
		static string? PATH_LOG = Android.App.Application.Context?.GetExternalFilesDir("")?.AbsolutePath.ToString();

	#elif IOS
		static string? PATH_LOG = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Library/Caches");
	#endif

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Logging
            .AddTraceLogger(
                options =>
                {
                    options.MinLevel = LogLevel.Trace;
                    options.MaxLevel = LogLevel.Critical;
                }) 
            .AddInMemoryLogger(
                options =>
                {
                    options.MaxLines = 1024;
                    options.MinLevel = LogLevel.Debug;
                    options.MaxLevel = LogLevel.Critical;
                })
            .AddStreamingFileLogger(
                options =>
                {
                    options.RetainDays = 1;
                    options.FolderPath = Path.Combine( PATH_LOG ?? "" );
                });

        #if DEBUG
		builder.Logging.AddDebug();
		Console.WriteLine(FileSystem.Current.AppDataDirectory.ToString());
        #endif
		builder.RegisterServices();
		builder.RegisterModels();
		builder.RegisterViews();
		return builder.Build();
	}


	public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
	{
		/*	
		mauiAppBuilder.Services.AddTransient<HttpClient>(o =>
		{
			return new HttpClient()
			{
				//BaseAddress = new Uri("http://10.100.8.12:8484/")
				BaseAddress = new Uri("http://10.100.8.12:5484")
			};
		});*/

		mauiAppBuilder.Services.AddTransient<HttpClient>();
    // Registrar HttpClient con nombre para el API principal
   
	
		mauiAppBuilder.Services.AddTransient<IMicroServices,MicroService>();
		mauiAppBuilder.Services.AddTransient<IFileServices,LocalService>();
		mauiAppBuilder.Services.AddTransient<IStorageServices,StorageService>();
		mauiAppBuilder.Services.AddTransient<ICypherServices,Aes256Service>();
		mauiAppBuilder.Services.AddTransient<IUserSessionServices,LogMicroService>();
		mauiAppBuilder.Services.AddTransient<IAccountMicroService,AccountMicroService>();
		mauiAppBuilder.Services.AddTransient<IManager,Manager>();

		return mauiAppBuilder;
	}

	public static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<LogModelToken>();
		mauiAppBuilder.Services.AddSingleton<ModelMicroService>();
		mauiAppBuilder.Services.AddSingleton<ModelAccountService>();
		mauiAppBuilder.Services.AddSingleton<ModelCredentialService>();

		return mauiAppBuilder;
	}

	public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<MainPage>();
		mauiAppBuilder.Services.AddSingleton<CommentPage>();
		mauiAppBuilder.Services.AddSingleton(LogOperatorRetriever.Instance);
		return mauiAppBuilder;
	}
}
