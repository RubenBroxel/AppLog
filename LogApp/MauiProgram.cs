using MetroLog.Operators;
using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Logging;
using LogApp.Services;
using LogApp.Services.ServicesManager;
using LogApp.Services.Security;
using LogApp.Services.Security.Contracts;


namespace LogApp;

public static class MauiProgram
{

	static string PATH_LOG="/storage/emulated/0/Android/data/com.demotechnical.logapp/";
	const string FILE_LOG_NAME = "DemoTechnical";

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
		
		builder.Services.AddTransient<HttpClient>(o =>
		{
			return new HttpClient()
			{
				BaseAddress = new Uri("http://10.100.8.5:8080/")
			};
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
                    options.FolderPath = Path.Combine( PATH_LOG, FILE_LOG_NAME );
                });

        #if DEBUG
		builder.Logging.AddDebug();
        #endif
		builder.RegisterServices();
		builder.RegisterViews();
		return builder.Build();
	}


	public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddTransient<HttpClient>(o =>
		{
			return new HttpClient()
			{
				BaseAddress = new Uri("http://10.100.8.5:8080/")
			};
		});
		mauiAppBuilder.Services.AddTransient<IFileServices,LocalServices>();
		mauiAppBuilder.Services.AddTransient<IFileServices,LocalServices>();
		mauiAppBuilder.Services.AddTransient<IMicroServices,MicroServices>();
		mauiAppBuilder.Services.AddTransient<IUserSessionServices,UserSessionServices>();
		mauiAppBuilder.Services.AddTransient<IManager,Manager>();

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
