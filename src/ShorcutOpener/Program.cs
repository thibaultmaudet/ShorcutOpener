using System;
using System.Collections.Generic;
using System.Text;

namespace ShorcutOpener
{
    public class Program
    {
        [System.STAThreadAttribute]
        public static void Main()
        {
            using (new ShorcutOpener.XamlIslandApp.App())
            {
                var app = new ShorcutOpener.App();
                app.InitializeComponent();
                app.Run();
            }
        }
    }
}
