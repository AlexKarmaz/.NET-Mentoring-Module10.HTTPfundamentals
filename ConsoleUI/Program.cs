using ConsoleUI.Restrictions;
using SiteDownloader;
using SiteDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo rootDirectory = new DirectoryInfo("F://New folder");
            IContentSaver contentSaver = new ContentSaver(rootDirectory);
            ILogger logger = new Logger(true);
            List<IRestriction> constraints = new List<IRestriction>();
            constraints.Add(new FileTypesRestriction("pdf, css, js".Select(e => "." + e)));
            constraints.Add(new CrossDomainRestriction(CrossDomainRestrictionType.All, new Uri("https://ru.wikipedia.org/wiki/%D0%9F%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5")));
            ISiteDownloader downloader = new Downloader(logger, contentSaver, constraints, 1);

            try
            {
                downloader.LoadFromUrl("https://ru.wikipedia.org/wiki/%D0%9F%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5");
            }
            catch (Exception ex)
            {
                logger.Log($"Error during site downloading: {ex.Message}");
            }
        }
    }
}
