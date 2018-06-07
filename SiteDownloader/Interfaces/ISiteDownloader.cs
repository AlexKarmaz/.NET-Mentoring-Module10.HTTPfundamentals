using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloader.Interfaces
{
    public interface ISiteDownloader
    {
        int MaxDeepLevel { get; set; }
        void LoadFromUrl(string url);
    }
}
