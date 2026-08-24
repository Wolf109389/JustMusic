using Microsoft.Extensions.Logging;
using JustMusic.ViewModels;
using JustMusic.Data;
using JustMusic.Services;
using CommunityToolkit.Maui;

namespace JustMusic
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement(isAndroidForegroundServiceEnabled: false)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<AppData>();

            builder.Services.AddSingleton<PlayListViewModel>();

            builder.Services.AddSingleton<MusicLibraryService>();   

            return builder.Build();
        }
    }
}
