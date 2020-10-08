using Dev2Be.Toolkit;
using MahApps.Metro.Controls;

using ShorcutOpener.Contracts.Views;
using ShorcutOpener.Core.Models;
using ShorcutOpener.Core.Services;
using ShorcutOpener.Helpers;
using ShorcutOpener.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Interop;
using Screen = System.Windows.Forms.Screen;

namespace ShorcutOpener.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        private readonly ShellViewModel shellViewModel;

        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            shellViewModel = viewModel;

            AssemblyInformations assemblyInformations = new AssemblyInformations(Assembly.GetExecutingAssembly().GetName().Name);

            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyInformations.Company, assemblyInformations.Product, "ShorcutOpener.Shorcuts.json")))
            {
                new FileService().Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyInformations.Company, assemblyInformations.Product), "ShorcutOpener.Shorcuts.json", new List<Shorcut>() { new Shorcut() { Text = "", Process = "" } });
            }

            Left = SetWindowLeftPosition();
            Top = SetWindowTopPosition();
        }
        
        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        private double SetWindowLeftPosition()
        {
            var screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            var dip1 = WindowsInteropHelper.TransformPixelsToDIP(this, screen.WorkingArea.X, 0);
            var dip2 = WindowsInteropHelper.TransformPixelsToDIP(this, screen.WorkingArea.Width, 0);
            var left = ((dip2.X - Width) / 2) + dip1.X;
            return left;
        }

        private double SetWindowTopPosition()
        {
            var screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            var dip1 = WindowsInteropHelper.TransformPixelsToDIP(this, 0, screen.WorkingArea.Y);
            var dip2 = WindowsInteropHelper.TransformPixelsToDIP(this, 0, screen.WorkingArea.Height);
            var top = ((dip2.Y - LauncherControl.ActualHeight) / 4) + dip1.Y;
            return top;
        }
    }
}
