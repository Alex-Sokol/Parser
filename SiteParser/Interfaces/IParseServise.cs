using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using DAL;
using NLog;

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
