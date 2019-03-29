using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SakashimaSystem.Models
{
    public class AppSettingsModel
    {
        public Cloudstorage CloudStorage { get; set; }
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class Cloudstorage
    {
        public string ConnectionString { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
    }
}
