using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using NLog.Fluent;
using SiteParser.Interfaces;
using StructureMap;

namespace serviceApp
{
    public class Checker
    {
        private readonly IParseServise _parseServise;
        private readonly Timer _timer;
        public Checker()
        {
            var c = Container.For<AppRegistry>();
            _parseServise = c.GetInstance<IParseServise>();
            _timer = new Timer(60000) {AutoReset = true};
           
        }

        public void Start()
        {
            _parseServise.Start();
            _timer.Elapsed += (sender, eventArgs) => Check();
            _timer.Start();

            using (IRepository<SiteForParsing> db = new Repository<SiteForParsing>("FirstAppDB"))
            {
                db.Create(new SiteForParsing
                {
                    Url = "http://www.deezer.com/",
                    ExternalLinks = false,
                    Depth = 5,
                    NumberOfThreads = 10,
                    Tree = true
                });
                db.Save();
            }
        }

        public void Check()
        {
            Console.WriteLine("Check");
            List<SiteForParsing> tasks = new List<SiteForParsing>();
            using (IRepository<SiteForParsing> db = new Repository<SiteForParsing>("FirstAppDB"))
            {
                tasks.AddRange(db.GetAll().ToList());
                if(tasks.Count != 0)
                    db.DeleteRange(tasks);
                db.Save();
            }

            if (tasks.Count == 0)
                return;

            foreach (var siteForParsing in tasks)
            {
                _parseServise.AddSiteForParsing(siteForParsing);
            }
        }
    }
}
