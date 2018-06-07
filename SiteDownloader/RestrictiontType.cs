using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloader
{
    [Flags]
    public enum RestrictionType
    {
        FileRestriction = 1,
        UrlRestriction = 2
    }
}
