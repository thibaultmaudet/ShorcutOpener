using System;
using System.Windows.Media;

using ShorcutOpener.Models;

namespace ShorcutOpener.Contracts.Services
{
    public interface IThemeSelectorService
    {
        event EventHandler ThemeChanged;

        SolidColorBrush GetColor(string colorKey);

        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
