using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleExample
{
    public class Settings
    {
        public string AppKey { get; private set; }
        public string CertificateLocation { get; private set; }
        public string CertificatePassword { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public Settings()
        {
            var settings = ConfigurationManager.AppSettings;

            this.AppKey = settings["AppKey"];
            this.CertificateLocation = settings["CertificateLocation"];
            this.CertificatePassword = settings["CertificatePassword"];
            this.Username = settings["Username"];
            this.Password = settings["Password"];
        }
    }
}
