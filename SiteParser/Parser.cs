using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using DAL.Entities;
using HtmlAgilityPack;
using SiteParser.Interfaces;

namespace SiteParser
{
    public class Parser : IParser
    {
        private readonly IPageManager _pageManager;

        public Parser(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        public string Domain { get; set; }
        public ConcurrentHashSet<string> LinksUrl { get; set; }
        public ConcurrentQueue<Page> PageQueue { get; set; }
        public Uri MainUrl { get; set; }
        public int NumberOfThreads { get; set; }
        public Site MainSite { get; set; }
        public Task[] Tasks { get; set; }

        public void Start(Site site, int numberOfThreads)
        {
            MainSite = site;
            NumberOfThreads = numberOfThreads;

            MainUrl = new Uri(site.Url);
            Domain = GetDomain.GetDomainFromUrl(MainUrl.OriginalString);

            PageQueue = new ConcurrentQueue<Page>();
            LinksUrl = new ConcurrentHashSet<string>();
            Tasks = new Task[numberOfThreads];

            ParsePage(new Page {Depth = 0, Url = MainUrl.OriginalString});

            for (var i = 0; i < NumberOfThreads; i++)
            {
                Tasks[i] = new Task(Parsing);
                Tasks[i].Start();
            }
        }

        private void Parsing()
        {
            while (!PageQueue.IsEmpty)
            {
                Page page;
                if (PageQueue.TryDequeue(out page))
                    ParsePage(page);
            }
        }

        private void ParsePage(Page p)
        {
            try
            {
                if (p.IsExternal && !MainSite.ExternalLinks)
                    return;

                var time = new Stopwatch();
                time.Start();
                var doc = _pageManager.DownloadPage(new Uri(p.Url));
                time.Stop();
                decimal ms = time.ElapsedMilliseconds;

                if (string.IsNullOrWhiteSpace(doc?.DocumentNode.OuterHtml))
                    return;

                AddPage(doc, p.Url, ms, p.Depth, p.IsExternal, p.ParentUrl);

                if (p.Depth >= MainSite.Depth) return;

                var links = _pageManager.GetLinks(doc, MainUrl);

                if (links == null)
                    return;

                foreach (var link in links)
                    if (LinksUrl.TryAdd(link))
                        PageQueue.Enqueue(new Page
                        {
                            Url = link,
                            Depth = p.Depth + 1,
                            IsExternal = IsExternal(link),
                            ParentUrl = p.Url
                        });
            }
            catch
            {
                //Console.WriteLine(e.Message);
            }
        }

        // Add Page to the List then save to DB
        private void AddPage(HtmlDocument doc, string url, decimal ms, int depth, bool isExternal, string parent)
        {
            lock (MainSite.Pages)
            {
                MainSite.Pages.Add(new Page
                {
                    Url = url,
                    Ping = ms,
                    Depth = depth,
                    IsExternal = isExternal,
                    Size = _pageManager.GetSize(doc),
                    CssFiles = _pageManager.GetCssFiles(doc),
                    Images = _pageManager.GetImages(doc),
                    SiteId = MainSite.Id,
                    ParentUrl = parent
                });
                Console.WriteLine($"{ms} ms - {depth} - {isExternal} - {url}");
            }
        }

        private bool IsExternal(string link)
        {
            return !link.Contains(Domain);
        }
    }
}