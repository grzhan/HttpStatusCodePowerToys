using ManagedCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Infrastructure;
using Wox.Infrastructure.Storage;
using Wox.Plugin;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.HttpStatusCodes
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IDisposable, ISettingProvider, ISavable
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

        private readonly PluginJsonStorage<PluginSettings> _storage;
        private readonly PluginSettings _settings;

        public Main()
        {
            _storage = new PluginJsonStorage<PluginSettings>();
            _settings = _storage.Load();
        }

        public void Save()
        {
            _storage.Save();
        }

        public Control CreateSettingPanel()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>()
        {
            new PluginAdditionalOption()
            {
                Key = "ReferenceType",
                DisplayLabel = "Reference Type",
                DisplayDescription =
                    "Configuration to determine the type of documentation (RFC or MDN) referenced upon pressing Enter or clicking to open the browser after finding the corresponding HTTP status code.",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Combobox,
                ComboBoxItems = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("RFC", "0"),
                    new KeyValuePair<string, string>("MDN", "1"),
                },
                ComboBoxValue = (int)_settings.ReferenceType
            }
        };

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
                            var url = httpStatus!.DefinedIn;
                            if (_settings.ReferenceType == ReferenceType.Mdn)
                            {
                                url = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/" + httpStatus!.Code;
                            }
                            
                            try {
                                if (Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern,
                                        url)) return true;
                            } catch (InvalidOperationException) {
                                // See https://github.com/grzhan/HttpStatusCodePowerToys/issues/3
                                // In some operating systems (perhaps Windows 10),
                                // the DefaultBrowserInfo fails to return the correct browser path.
                                // Therefore, attempt to launch the browser directly based on the URL.
                                Process.Start(new ProcessStartInfo(url)
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

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            var refOption = 0;
            if (settings is { AdditionalOptions: not null })
            {
                var referenceType =
                    settings.AdditionalOptions.FirstOrDefault(x => x.Key == "ReferenceType");
                refOption = referenceType?.ComboBoxValue ?? refOption;
                _settings.ReferenceType = (ReferenceType)refOption;
            }

            Save();
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme is Theme.Light or Theme.HighContrastWhite ? "Images/httpstatuscodes.light.png" : "Images/httpstatuscodes.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);
    }
}
