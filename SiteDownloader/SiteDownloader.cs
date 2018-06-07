using SiteDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace SiteDownloader
{
    public class Downloader : ISiteDownloader
    {
        private readonly ILogger logger;
        private readonly IContentSaver contentSaver;
        private readonly List<IRestriction> urlRestrictions;
        private readonly List<IRestriction> fileRestrictions;
        private const string HtmlDocumentMediaType = "text/html";
        private readonly ISet<Uri> _visitedUrls = new HashSet<Uri>();

        public int MaxDeepLevel { get; set; }

        public Downloader(ILogger logger, IContentSaver contentSaver, IEnumerable<IRestriction> restrictions, int maxDeepLevel = 0)
        {
            if (maxDeepLevel < 0)
            {
                throw new ArgumentException($"{nameof(maxDeepLevel)} cannot be less than zero");
            }

            this.logger = logger;
            this.contentSaver = contentSaver;
            this.urlRestrictions = restrictions.Where(c => (c.RestrictionType & RestrictionType.UrlRestriction) != 0).ToList();
            this.fileRestrictions = restrictions.Where(c => (c.RestrictionType & RestrictionType.FileRestriction) != 0).ToList();

            MaxDeepLevel = maxDeepLevel;
        }

        public void LoadFromUrl(string url)
        {
            _visitedUrls.Clear();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                ScanUrl(httpClient, httpClient.BaseAddress, 0);
            }
        }

        private void ScanUrl(HttpClient httpClient, Uri uri, int level)
        {
            if (level > MaxDeepLevel || _visitedUrls.Contains(uri) || !IsValidScheme(uri.Scheme))
            {
                return;
            }
            _visitedUrls.Add(uri);
            HttpResponseMessage head = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri)).Result;

            if (!head.IsSuccessStatusCode)
            {
                return;
            }

            if (head.Content.Headers.ContentType?.MediaType == HtmlDocumentMediaType)
            {
                ProcessHtmlDocument(httpClient, uri, level);
            }
            else
            {
                ProcessFile(httpClient, uri);
            }
        }

        private void ProcessFile(HttpClient httpClient, Uri uri)
        {
            logger.Log($"File founded: {uri}");
            if (!IsAcceptableUri(uri, fileRestrictions))
            {
                return;
            }

            var response = httpClient.GetAsync(uri).Result;
            logger.Log($"File loaded: {uri}");
            contentSaver.SaveFile(uri, response.Content.ReadAsStreamAsync().Result);
        }

        private void ProcessHtmlDocument(HttpClient httpClient, Uri uri, int level)
        {
            logger.Log($"Url founded: {uri}");
            if (!IsAcceptableUri(uri, urlRestrictions))
            {
                return;
            }

            var response = httpClient.GetAsync(uri).Result;
            var document = new HtmlDocument();
            document.Load(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
            logger.Log($"Html loaded: {uri}");
            contentSaver.SaveHtmlDocument(uri, GetDocumentFileName(document), GetDocumentStream(document));

            var attributesWithLinks = document.DocumentNode.Descendants().SelectMany(d => d.Attributes.Where(IsAttributeWithLink));
            foreach (var attributesWithLink in attributesWithLinks)
            {
                ScanUrl(httpClient, new Uri(httpClient.BaseAddress, attributesWithLink.Value), level + 1);
            }
        }

        private bool IsAttributeWithLink(HtmlAttribute attribute)
        {
            return attribute.Name == "src" || attribute.Name == "href";
        }

        private string GetDocumentFileName(HtmlDocument document)
        {
            return document.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText + ".html";
        }

        private Stream GetDocumentStream(HtmlDocument document)
        {
            MemoryStream memoryStream = new MemoryStream();
            document.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private bool IsValidScheme(string scheme)
        {
            if (scheme.Equals("http") || scheme.Equals("https"))
            {
                return true;
            }

            return false;
        }

        private bool IsAcceptableUri(Uri uri, IEnumerable<IRestriction> restrictions)
        {
            return restrictions.All(c => c.IsAcceptable(uri));
        }
    }
}
