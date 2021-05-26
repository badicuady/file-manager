using System.Collections.Generic;

namespace FileManager.Shared.Settings
{
    public class FileManagerCorsSettings
    {
        public List<string> HostOrigins
        {
            get;
            set;
        }

        public List<string> AllowedMethods
        {
            get;
            set;
        }

        public List<string> AllowedHeaders
        {
            get;
            set;
        }
    }
}
