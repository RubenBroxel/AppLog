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
                    options.FolderPath = Path.Combine( PATH_LOG );
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
		mauiAppBuilder.Services.AddTransient<HttpClient>(o =>
		{
			return new HttpClient()
			{
				BaseAddress = new Uri("http://10.100.8.4:8080/")
			};
		});
	
		mauiAppBuilder.Services.AddTransient<IMicroServices,MicroServices>();
		mauiAppBuilder.Services.AddTransient<IFileServices,LocalServices>();
		mauiAppBuilder.Services.AddTransient<IStorageService,StorageService>();
		mauiAppBuilder.Services.AddTransient<IUserSessionServices,UserSessionServices>();
		mauiAppBuilder.Services.AddTransient<IManager,Manager>();

		return mauiAppBuilder;
	}

	public static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<JsonLogToken>();
		mauiAppBuilder.Services.AddSingleton<LogLocal>();
		mauiAppBuilder.Services.AddSingleton<LogMicroService>();
		mauiAppBuilder.Services.AddSingleton<UserCredentials>();

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
