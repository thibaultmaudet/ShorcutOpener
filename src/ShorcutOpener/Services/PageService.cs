using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using ShorcutOpener.Contracts.Services;
using ShorcutOpener.Helpers;
using ShorcutOpener.ViewModels;
using ShorcutOpener.Views;

namespace ShorcutOpener.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<string, Type> pages = new Dictionary<string, Type>();
        private readonly IServiceProvider serviceProvider;

        public PageService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Configure<LauncherViewModel, LauncherPage>();
        }

        public Type GetPageType(string key)
        {
            Type pageType;
            lock (pages)
            {
                if (!pages.TryGetValue(key, out pageType))
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }

            return pageType;
        }

        public Page GetPage(string key)
        {
            var pageType = GetPageType(key);
            return serviceProvider.GetService(pageType) as Page;
        }

        private void Configure<VM, V>() where VM : Observable where V : Page
        {
            lock (pages)
            {
                var key = typeof(VM).FullName;

                if (pages.ContainsKey(key))
                    throw new ArgumentException($"The key {key} is already configured in PageService");

                var type = typeof(V);

                if (pages.Any(p => p.Value == type))
                    throw new ArgumentException($"This type is already configured with key {pages.First(p => p.Value == type).Key}");

                pages.Add(key, type);
            }
        }
    }
}
