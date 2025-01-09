using ManagedCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wox.Infrastructure;
using Wox.Plugin;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.HttpStatusCodes
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IDisposable
    {
        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "8419E21B985441A9A06C4B6264422694";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Http Status Codes";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Search for HTTP status codes and open the corresponding RFC.";

        private PluginInitContext Context { get; set; }

        private string IconPath { get; set; }

        private bool Disposed { get; set; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            ArgumentNullException.ThrowIfNull(query);
            var isGlobalQuery = string.IsNullOrEmpty(query.ActionKeyword);
            if (string.IsNullOrEmpty(query.Search) || isGlobalQuery || query.Search.Length < 3)
            {
                return [];
            }
            if (HttpStatusCatalog.TryFindByCode(query.Search, out var httpStatus))
            {
                return [
                    new Result
                    {
                        Title = $"{httpStatus?.Code} {httpStatus?.ReasonPhrase}",
                        SubTitle = httpStatus?.OneLiner,
                        IcoPath = IconPath,
                        Action = _ =>
                        {
                            try {
                                if (Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern,
                                        httpStatus?.DefinedIn)) return true;
                            } catch (InvalidOperationException) {
                                // See https://github.com/grzhan/HttpStatusCodePowerToys/issues/3
                                // In some operating systems (perhaps Windows 10),
                                // the DefaultBrowserInfo fails to return the correct browser path.
                                // Therefore, attempt to launch the browser directly based on the URL.
                                Process.Start(new ProcessStartInfo(httpStatus!.DefinedIn)
                                {
                                    UseShellExecute = true
                                });
                            }
                            ErrorHandler.OnPluginError();
                            return false;
                        },
                        Score = 300,
                    }
                ];
            }
            return [];
        }

        /// <summary>
        /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
        public void Init(PluginInitContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Wrapper method for <see cref="Dispose()"/> that dispose additional objects and events form the plugin itself.
        /// </summary>
        /// <param name="disposing">Indicate that the plugin is disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed || !disposing)
            {
                return;
            }

            if (Context?.API != null)
            {
                Context.API.ThemeChanged -= OnThemeChanged;
            }

            Disposed = true;
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme is Theme.Light or Theme.HighContrastWhite ? "Images/httpstatuscodes.light.png" : "Images/httpstatuscodes.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);
    }
}
