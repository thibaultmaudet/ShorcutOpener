using ShorcutOpener.ViewModels;
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
using System.Windows.Shapes;

namespace ShorcutOpener.Views
{
    /// <summary>
    /// Logique d'interaction pour LauncherWindow.xaml
    /// </summary>
    public partial class LauncherPage : Page
    {
        public LauncherPage(LauncherViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
