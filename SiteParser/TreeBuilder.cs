using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using SiteParser.Interfaces;

namespace SiteParser
{
    public class TreeBuilder : ITreeBuilder
    {
        public TreeBuilder()
        {
            Tree = new StringBuilder();
            DocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public Site Site { get; set; }
        public string Url { get; set; }
        public StringBuilder Tree { get; set; }
        public List<Page> Pages { get; set; }
        public Page FirstPage { get; set; }
        public string DocPath { get; set; }
        public string DocName { get; set; }

        public void Build(Site site)
        {
            Site = site;
            Url = site.Url;
            DocName = @"\" + GetDomain.GetDomainFromUrl(Url) + ".txt";

            using (IRepository<Page> db = new Repository<Page>("FirstAppDB"))
            {
                Pages = db.Find(page => page.SiteId == Site.Id).ToList();
            }

            Begin();
        }

        public void Build(string url)
        {
            Url = url;
            DocName = @"\" + GetDomain.GetDomainFromUrl(Url) + ".txt";

            using (IRepository<Page> db = new Repository<Page>("FirstAppDB"))
            {
                var page = db.Find(p => p.Url == Url).FirstOrDefault();
                FirstPage = page;
                if (page == null)
                    return;
                Pages = db.Find(p => p.SiteId == page.SiteId && p.Depth > page.Depth).ToList();
            }

            Begin();
        }

        private void Begin()
        {
            if (Pages == null)
                return;

            using (var writer = new StreamWriter(DocPath + DocName))
            {
                writer.WriteLine($"Site: {Url}");
                writer.WriteLine($"Total number of pages: {Pages.Count}");
            }

            if (FirstPage == null)
                FirstPage = (from x in Pages
                    where x.Depth == 0
                    select x).FirstOrDefault();

            Add(FirstPage);
        }

        private void Add(Page page)
        {
            if (page == null)
                return;

            using (var writer = new StreamWriter(DocPath + DocName, true))
            {
                writer.WriteLine($"|{new string('-', page.Depth)}{page.Url}");
            }

            var pages = (from x in Pages
                where x.ParentUrl == page.Url
                select x).ToList();

            if (pages.Count == 0) return;

            foreach (var p in pages)
                Add(p);
        }
    }
}