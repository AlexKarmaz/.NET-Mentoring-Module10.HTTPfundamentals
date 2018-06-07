using SiteDownloader;
using SiteDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Restrictions
{
    public class CrossDomainRestriction : IRestriction
    {
        private readonly Uri parentUri;
        private readonly CrossDomainRestrictionType restrictionType;

        public CrossDomainRestriction(CrossDomainRestrictionType restrictionType, Uri parentUri)
        {
            switch (restrictionType)
            {
                case CrossDomainRestrictionType.All:
                case CrossDomainRestrictionType.CurrentDomainOnly:
                case CrossDomainRestrictionType.DescendantUrlsOnly:
                    this.restrictionType = restrictionType;
                    this.parentUri = parentUri;
                    break;
                default:
                    throw new ArgumentException($"Unknown transition type: {restrictionType}");
            }
        }

        public RestrictionType RestrictionType => RestrictionType.UrlRestriction | RestrictionType.FileRestriction;

        public bool IsAcceptable(Uri uri)
        {
            switch (restrictionType)
            {
                case CrossDomainRestrictionType.All:
                    return true;
                case CrossDomainRestrictionType.CurrentDomainOnly:
                    if (parentUri.DnsSafeHost == uri.DnsSafeHost)
                    {
                        return true;
                    }
                    break;
                case CrossDomainRestrictionType.DescendantUrlsOnly:
                    if (parentUri.IsBaseOf(uri))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}
