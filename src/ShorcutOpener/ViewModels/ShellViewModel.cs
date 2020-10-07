using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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

            AssemblyInformations assemblyInformations = new AssemblyInformations(Assembly.GetExecutingAssembly().GetName().Name);

            shorcuts = new FileService().Read<List<Shorcut>>(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyInformations.Company, assemblyInformations.Product), "ShorcutOpener.Shorcuts.json");
        }

        private void InitializeKeyCommands()
        {
            IgnoreCommand = new RelayCommand<object>(_ => { });

            EscapeCommand = new RelayCommand<object>(_ =>
            {
                
            });

            ValidateCommand = new RelayCommand<object>(_ =>
            {
                if (text.Length > 0 && shorcuts.Count > 0)
                {
                    Shorcut shorcut = shorcuts.Find(x => x.Text.Equals(text, StringComparison.InvariantCultureIgnoreCase));

                    if (shorcut != null)
                        Process.Start(new ProcessStartInfo(shorcut.Process) { UseShellExecute = true, Arguments = shorcut.Argument });
                }
            });
        }
    }
}
