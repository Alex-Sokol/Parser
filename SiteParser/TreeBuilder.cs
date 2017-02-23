using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using SiteParser.Interfaces;

namespace SiteParser
{
    public class TreeBuilder : ITreeBuilder
    {
        public Site Site { get; set; }
        public string Url { get; set; }
        public StringBuilder Tree { get; set; }
        public StreamWriter Writer { get; set; }
        public List<Page> Pages { get; set; }
        public string DocPath { get; set; }

        public TreeBuilder()
        {
            Tree = new StringBuilder();
            DocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public void Build(Site site)
        {
            Site = site;
            Url = site.Url;
            Writer = new StreamWriter(DocPath + $"\\{GetDomain.GetDomainFromUrl(Url)}.txt");

            using (IRepository<Page> db = new Repository<Page>("FirstAppDB"))
            {
                Pages = db.Find(page => page.SiteId == Site.Id).ToList();
            }

            Begin();
        }

        public void Build(string url)
        {
            Url = url;
            Writer = new StreamWriter(DocPath + $"\\{GetDomain.GetDomainFromUrl(Url)}.txt");

            using (IRepository<Page> db = new Repository<Page>("FirstAppDB"))
            {
                var page = db.Find(p => p.Url == Url).FirstOrDefault();
                if (page == null)
                    return;
                Pages = db.Find(p => p.SiteId == page.SiteId && p.Depth >= page.Depth).ToList();
            }

            Begin();
        }
        public void Begin()
        {
            if (Pages == null)
                return;

            Tree.AppendLine($"Site: {Url}");
            Tree.AppendLine($"Total number of pages: {Pages.Count}");

            var firstPage = (from x in Pages
                             where x.Url == Url
                             select x).FirstOrDefault();

            Add(firstPage);

            SaveInFile();
            Writer.Dispose();
        }

        private void SaveInFile()
        {
            Writer.WriteLine(Tree);
            Tree.Clear();
        }

        private void Add(Page page)
        {
            if (page == null)
                return;

            if (Tree.Capacity > 1000000)
            {
                SaveInFile();
            }

            Tree.AppendLine($"|{new string('-', page.Depth)}{page.Url}");

            var pages = (from x in Pages
                         where x.ParentUrl == page.Url
                         select x).ToList();

            if (pages.Count == 0) return;

            foreach (var p in pages)
                Add(p);
        }
    }
}