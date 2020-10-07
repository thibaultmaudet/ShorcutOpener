using Dev2Be.Toolkit;
using MahApps.Metro.Controls;

using ShorcutOpener.Contracts.Views;
using ShorcutOpener.Core.Models;
using ShorcutOpener.Core.Services;
using ShorcutOpener.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ShorcutOpener.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            AssemblyInformations assemblyInformations = new AssemblyInformations(Assembly.GetExecutingAssembly().GetName().Name);

            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyInformations.Company, assemblyInformations.Product, "ShorcutOpener.Shorcuts.json")))
            {
                new FileService().Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyInformations.Company, assemblyInformations.Product), "ShorcutOpener.Shorcuts.json", new List<Shorcut>() { new Shorcut() { Text = "", Process = "" } });
            }
        }

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}
