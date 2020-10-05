using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace ShorcutOpener.XamlIsland
{
    public sealed partial class LauncherControlUniversal : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool UseDarkTheme
        {
            get { return (bool)GetValue(UseDarkThemeProperty); }
            set { SetValue(UseDarkThemeProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(LauncherControlUniversal), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty UseDarkThemeProperty = DependencyProperty.Register(nameof(UseDarkTheme), typeof(bool), typeof(LauncherControlUniversal), new PropertyMetadata(false, OnUseDarkThemePropertyChanged));

        public LauncherControlUniversal()
        {
            InitializeComponent();
        }

        private static void OnUseDarkThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LauncherControlUniversal control && e.NewValue is bool useDarkTheme)
                control.RequestedTheme = useDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
        }
    }
}
