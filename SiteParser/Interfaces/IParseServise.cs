using DAL.Entities;

namespace SiteParser.Interfaces
{
    public interface IParseServise
    {
        bool IsParsing { get; set; }

        void AddSiteForParsing(SiteForParsing site);
        void AddSiteForParsing(string url, int depth, bool externalLinks, int threads, bool tree = true);

        void Run(SiteForParsing siteForParsing);
        void Start();
    }
}