using SiteDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Logger : ILogger
    {
        private readonly bool isLogEnabled;

        public Logger(bool isLogEnabled)
        {
            isLogEnabled = isLogEnabled;
        }

        public void Log(string message)
        {
            if (isLogEnabled)
            {
                Console.WriteLine(message);
            }
        }
    }
}
