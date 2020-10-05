using ShorcutOpener.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShorcutOpener.ViewModels
{
    public class LauncherViewModel : Observable
    {
        private string text;

        public string Text
        {
            get { return text; }
            set { Set(ref text, value); }
        }
    }
}
