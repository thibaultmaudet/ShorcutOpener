using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Dev2Be.Toolkit;
using ShorcutOpener.Core.Models;
using ShorcutOpener.Core.Services;
using ShorcutOpener.Helpers;

namespace ShorcutOpener.ViewModels
{
    public class ShellViewModel : Observable
    {
        private string text;

        private List<Shorcut> shorcuts;

        private readonly List<string> internalCommands;

        public ICommand IgnoreCommand { get; set; }

        public ICommand EscapeCommand { get; set; }
        public ICommand ValidateCommand { get; set; }
        
        public string Text
        {
            get { return text; }
            set { Set(ref text, value); }
        }

        public ShellViewModel()
        {
            InitializeKeyCommands();

            shorcuts = Reload();

            internalCommands = new List<string>() { "edit", "exit", "reload" };
        }

        private void InitializeKeyCommands()
        {
            IgnoreCommand = new RelayCommand<object>(_ => { });

            EscapeCommand = new RelayCommand<object>(_ =>
            {
                
            });

            ValidateCommand = new RelayCommand<object>(_ =>
            {
                Shorcut shorcut = null;

                if (text.Length > 0 && shorcuts.Count > 0)
                {
                    shorcut = shorcuts.Find(x => x.Text.Equals(text, StringComparison.InvariantCultureIgnoreCase));

                    if (shorcut != null)
                        Process.Start(new ProcessStartInfo(shorcut.Process) { UseShellExecute = true, Arguments = shorcut.Argument });
                }
                
                if (text.Length > 0 && shorcut == null)
                {
                    string command = internalCommands.Find(x => x.Equals(text, StringComparison.InvariantCultureIgnoreCase));

                    switch (command)
                    {
                        case "edit":
                            AssemblyInformations assemblyInformations = new AssemblyInformations(Assembly.GetExecutingAssembly().GetName().Name);
                            Process.Start(new ProcessStartInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyInformations.Company, assemblyInformations.Product, "ShorcutOpener.Shorcuts.json")) { UseShellExecute = true });
                            break;
                        case "exit":
                            Application.Current.Shutdown();
                            break;
                        case "reload":
                            shorcuts = Reload();
                            break;
                        default:
                            break;
                    }
                }
            });
        }

        private List<Shorcut> Reload()
        {
            AssemblyInformations assemblyInformations = new AssemblyInformations(Assembly.GetExecutingAssembly().GetName().Name);

            return new FileService().Read<List<Shorcut>>(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyInformations.Company, assemblyInformations.Product), "ShorcutOpener.Shorcuts.json");
        }
    }
}
