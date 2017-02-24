using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using NLog;
using SiteParser.Interfaces;

namespace SiteParser
{
    public class ParseService : IParseServise
    {
        public static Logger Log;
        private readonly IParser _parser;
        private readonly ITreeBuilder _treeBuilder;

        private ActionBlock<SiteForParsing> _actionBlock;

        public ParseService(IParser parser, ITreeBuilder treeBuilder)
        {
            _parser = parser;
            _treeBuilder = treeBuilder;
        }

        public bool IsParsing { get; set; }

        public void AddSiteForParsing(SiteForParsing site)
        {
            _actionBlock.Post(site);
        }

        public void AddSiteForParsing(string url, int depth, bool externalLinks, int threads, bool tree = true)
        {
            _actionBlock.Post(new SiteForParsing
            {
                Url = url,
                Depth = depth,
                ExternalLinks = externalLinks,
                NumberOfThreads = threads,
                Tree = tree
            });
        }

        public void Start()
        {
            _actionBlock = new ActionBlock<SiteForParsing>(parsing => { Run(parsing); },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                });
        }

        public void Run(SiteForParsing siteForParsing)
        {
            try
            {
                Log = LogManager.GetCurrentClassLogger();

                var site = new Site
                {
                    Url = siteForParsing.Url,
                    ExternalLinks = siteForParsing.ExternalLinks,
                    Depth = siteForParsing.Depth,
                    LastUpdate = DateTime.Now
                };

                SaveSiteToDb(site);

                Log.Info($"Run parsing: {site.Url}");
                var time = new Stopwatch();
                time.Start();

                IsParsing = true;
                _parser.Start(site, siteForParsing.NumberOfThreads);
                var save = new Task(SavePagesToDb);
                save.Start();

                Task.WaitAll(_parser.Tasks);
                IsParsing = false;
                Log.Info("Parsing Complete");
                Task.WaitAll(save);

                time.Stop();

                Log.Info($"Total time: {time.ElapsedMilliseconds}");
                Console.WriteLine($@"Total Pages: {_parser.MainSite.Pages.Count} Time: {time.ElapsedMilliseconds}");

                if (!siteForParsing.Tree) return;

                _treeBuilder.Build(_parser.MainSite);
                Log.Info("Tree was successfully builded");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }


        private void SavePagesToDb()
        {
            while (true)
            {
                Thread.Sleep(500);

                var pages = new List<Page>();
                lock (_parser.MainSite.Pages)
                {
                    pages.AddRange(_parser.MainSite.Pages);
                    _parser.MainSite.Pages.Clear();
                }

                using (IRepository<Page> db = new Repository<Page>("FirstAppDB"))
                {
                    foreach (var page in pages)
                        db.AddOrUpdate(x => new {x.SiteId, x.Url}, page);

                    db.Save();
                }

                if (!IsParsing && _parser.MainSite.Pages.Count == 0)
                    break;
            }
        }

        private void SaveSiteToDb(Site site)
        {
            using (IRepository<Site> db = new Repository<Site>("FirstAppDB"))
            {
                db.AddOrUpdate(x => x.Url, site);
                db.Save();
            }
        }
    }
}