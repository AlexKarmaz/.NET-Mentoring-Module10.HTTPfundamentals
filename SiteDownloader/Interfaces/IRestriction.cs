using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloader.Interfaces
{
    public interface IRestriction
    {
        RestrictionType RestrictionType { get; }
        bool IsAcceptable(Uri uri);
    }
}
