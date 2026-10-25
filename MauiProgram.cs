using System;
using CarPoolIndia.Services;
using CarPoolIndia.ViewModels;
using CarPoolIndia.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace CarPoolIndia;

public static class MauiProgram
{
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

        // 1. Refit HTTP Client Setup (pointing to production or staging CarPool India API)
        builder.Services.AddRefitClient<ICatalogApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.carpoolindia.internal"));

        // 2. Catalog Service (Using MockCatalogService for offline & seeded live pools demonstration)
        builder.Services.AddSingleton<ICatalogService, MockCatalogService>();

        // 3. MVVM Community Toolkit ViewModels
        builder.Services.AddTransient<DealsViewModel>();
        builder.Services.AddTransient<BookingsViewModel>();
        builder.Services.AddTransient<SupportViewModel>();

        // 4. Views / Pages
        builder.Services.AddTransient<DealsPage>();
        builder.Services.AddTransient<BookingsPage>();
        builder.Services.AddTransient<SupportPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
