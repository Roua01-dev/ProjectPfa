using Microsoft.Extensions.Logging;
using ProjectPfa.View;
using ProjectPfa.ViewModel;

namespace ProjectPfa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).ConfigureEssentials(essentials =>
                {
                    essentials.UseMapServiceToken("uZdBSIZocMFY5qSbY067~AabHd10R2Z1LuAUSJjiKow~Ag1CpHQxSx_l1pd4oVIP2JBiAo5m3TlvVkLSD-sKxLj6fepYS8GX8POjlnBoj0J2");
                });
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<IMap>(Map.Default);
            builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
