using SiteDownloader;
using SiteDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Restrictions
{
    public class FileTypesRestriction : IRestriction
    {
        private readonly IEnumerable<string> acceptableExtensions;

        public FileTypesRestriction(IEnumerable<string> acceptableExtensions)
        {
            this.acceptableExtensions = acceptableExtensions;
        }

        public RestrictionType RestrictionType => RestrictionType.FileRestriction;

        public bool IsAcceptable(Uri uri)
        {
            string lastSegment = uri.Segments.Last();
            return acceptableExtensions.Any(e => lastSegment.EndsWith(e));
        }
    }
}
