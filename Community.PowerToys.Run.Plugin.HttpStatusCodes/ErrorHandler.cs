using System;
using System.Collections.Generic;
using Wox.Plugin;
using Wox.Plugin.Logger;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.HttpStatusCodes;

internal static class ErrorHandler
{
    internal static List<Result> OnError(string icon, string queryInput, string errorMessage, Exception exception = default)
    {
        Log.Error($"Failed to calculate <{queryInput}>: {errorMessage}", typeof(HttpStatusCodes.Main));
 
        return [ CreateErrorResult(errorMessage, icon) ];
    }

    internal static void OnPluginError()
    {
        var errorMessage = $"Failed to open {BrowserInfo.Name ?? BrowserInfo.MSEdgeName}";
        Log.Error(errorMessage, typeof(HttpStatusCodes.Main));
    }
    
    private static Result CreateErrorResult(string errorMessage, string iconPath)
    {
        return new Result
        {
            Title = "Search failed",
            SubTitle = errorMessage,
            IcoPath = iconPath,
            Score = 300,
        };
    }
}
