namespace Animato.Messaging.Application.Common;


public static class GlobalOptionsExtensions
{
    public static bool UseApplicationInsights(this GlobalOptions globalOptions)
        => !string.IsNullOrEmpty(globalOptions.ApplicationInsightsKey);
}
