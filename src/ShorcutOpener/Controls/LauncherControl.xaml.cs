using Microsoft.Toolkit.Wpf.UI.XamlHost;
using ShorcutOpener.Contracts.Services;
using ShorcutOpener.Models;
using ShorcutOpener.XamlIsland;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShorcutOpener.Controls
{
    /// <summary>
    /// Logique d'interaction pour LauncherControl.xaml
    /// </summary>
    public partial class LauncherControl : UserControl
    {
        private readonly IThemeSelectorService themeSelectorService;

        private bool useDarkTheme;
        private LauncherControlUniversal launcherControl;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(LauncherControl), new PropertyMetadata(string.Empty));

        public LauncherControl()
        {
            InitializeComponent();

            themeSelectorService = ((App)Application.Current).GetService<IThemeSelectorService>();
            themeSelectorService.ThemeChanged += OnThemeChanged;
            GetColors();
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            GetColors();
            ApplyColors();
        }

        private void OnChildChanged(object sender, EventArgs e)
        {
            if (sender is WindowsXamlHost host && host.GetUwpInternalObject() is LauncherControlUniversal xamlIsland)
            {
                launcherControl = xamlIsland;
                ApplyColors();

                launcherControl.SetBinding(LauncherControlUniversal.TextProperty, new Windows.UI.Xaml.Data.Binding() { Path = new Windows.UI.Xaml.PropertyPath(nameof(Text)), Mode = Windows.UI.Xaml.Data.BindingMode.TwoWay });
            }
        }

        private void GetColors()
        {
            useDarkTheme = themeSelectorService.GetCurrentTheme() == AppTheme.Dark;
        }

        private void ApplyColors()
        {
            launcherControl.UseDarkTheme = useDarkTheme;
        }
    }
}
