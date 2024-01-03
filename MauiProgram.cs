using MauiCookbook.Services;
using Microsoft.Extensions.Logging;
using MobileCookbook.Services;
using CommunityToolkit.Maui;

namespace MobileCookbook
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            }).UseMauiCommunityToolkit();
            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IRecipeService, RecipeService>();
            builder.Services.AddSingleton<IShoppingListService, ShoppingListService>();
            builder.Services.AddSingleton<IPlanningService, PlanningService>();
            builder.Services.AddSingleton<RecipeDatabase>();
            return builder.Build();
        }
    }
}