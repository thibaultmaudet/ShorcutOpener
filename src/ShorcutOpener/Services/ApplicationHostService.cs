using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using ShorcutOpener.Contracts.Services;
using ShorcutOpener.Contracts.Views;
using ShorcutOpener.ViewModels;

namespace ShorcutOpener.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly INavigationService navigationService;
        private readonly IPersistAndRestoreService persistAndRestoreService;
        private readonly IThemeSelectorService themeSelectorService;
        private IShellWindow shellWindow;

        public ApplicationHostService(IServiceProvider serviceProvider, INavigationService navigationService, IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService)
        {
            this.serviceProvider = serviceProvider;
            this.navigationService = navigationService;
            this.themeSelectorService = themeSelectorService;
            this.persistAndRestoreService = persistAndRestoreService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            await HandleActivationAsync();

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            persistAndRestoreService.PersistData();
            await Task.CompletedTask;
        }

        private async Task InitializeAsync()
        {
            persistAndRestoreService.RestoreData();
            themeSelectorService.SetTheme();
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            await Task.CompletedTask;
        }

        private async Task HandleActivationAsync()
        {
            if (App.Current.Windows.OfType<IShellWindow>().Count() == 0)
            {
                // Default activation that navigates to the apps default page
                shellWindow = serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
                navigationService.Initialize(shellWindow.GetNavigationFrame());
                shellWindow.ShowWindow();
                navigationService.NavigateTo(typeof(LauncherViewModel).FullName);
                await Task.CompletedTask;
            }
        }
    }
}
