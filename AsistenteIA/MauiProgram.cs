using Microsoft.Extensions.Logging;
using AsistenteIA.Services;
using AsistenteIA.ViewModels;
using AsistenteIA.Views;
using CommunityToolkit.Maui;


namespace AsistenteIA;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Registro de Servicios y ViewModels
        builder.Services.AddSingleton<ApiKeyService>();
        builder.Services.AddSingleton<AiService>();
        builder.Services.AddSingleton<DatabaseService>();

        builder.Services.AddTransient<ChatViewModel>();
        builder.Services.AddTransient<ChatPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}