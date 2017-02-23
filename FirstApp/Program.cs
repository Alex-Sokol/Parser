using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using SiteParser;
using SiteParser.Interfaces;
using StructureMap;

namespace FirstApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var c = Container.For<AppRegistry>();
            var s = c.GetInstance<IParseServise>();

            //using (IRepository<SiteForParsing> db = new Repository<SiteForParsing>("FirstAppDB"))
            //{
            //    db.Create(new SiteForParsing
            //    {
            //        Url = "https://wikipedia.org/",
            //        ExternalLinks = false,
            //        Depth = 1,
            //        NumberOfThreads = 10,
            //        Tree = true
            //    });
            //    db.Save();
            //}

            s.Start();
            //s.AddSiteForParsing("http://www.ok-studio.com.ua/", 10, false, 10);
            //s.AddSiteForParsing("https://www.google.com", 1, false, 10);
            //s.AddSiteForParsing("https://wikipedia.org/", 1, false, 10);
            s.AddSiteForParsing("http://www.deezer.com/", 2, false, 10);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main menu\nChoose the operation:\n 1. Parse site\n 2. Build tree\n 3. Show sites\n 4. Exit");
                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        Console.Clear();
                        AddSite(s);
                        break;
                    case "2":
                        Console.Clear();
                        BuildTree();
                        break;
                    case "3":
                        Console.Clear();
                        GetListOfSites();
                        Console.ReadKey();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static void AddSite(IParseServise service)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Input starting url");
                    string url = Console.ReadLine();

                    Console.WriteLine("Input depth");
                    int depth;
                    int.TryParse(Console.ReadLine(), out depth);

                    Console.WriteLine("Input number of threads");
                    int threads;
                    int.TryParse(Console.ReadLine(), out threads);

                    Console.WriteLine("Parse external links? 1 - Yes; 2 -  No.");
                    string ex = Console.ReadLine();
                    bool external = ex == "1";

                    service.AddSiteForParsing(url, depth, external, threads);
                    Console.Clear();
                    Console.WriteLine("Site has been added to queue");
                    Console.ReadKey();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
            }
        }

        private static void BuildTree()
        {
            while (true)
            {
                ITreeBuilder tb = new TreeBuilder();
                Console.Clear();
                Console.WriteLine("Tree\nChoose the operation:\n 1. All site\n 2. Part of site(using url)\n 3. Cancel");
                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Choose the site:");

                        var sites = GetListOfSites();
                        int id;
                        int.TryParse(Console.ReadLine(), out id);

                        var res = (from x in sites
                            where x.Id == id
                            select x).FirstOrDefault();

                        if (res != null)
                        {
                            tb.Build(res);
                            Console.Clear();
                            Console.WriteLine("Complete!");
                            Console.ReadKey();
                        }
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Input starting Url");
                        string url = Console.ReadLine();
                        
                        tb.Build(url);
                        Console.Clear();
                        Console.WriteLine("Complete!");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Clear();
                        return;
                }
            }
        }

        public static List<Site> GetListOfSites()
        {
            List<Site> sites;
            using (IRepository<Site> db = new Repository<Site>("FirstAppDB"))
            {
                sites = db.GetAll().ToList();
            }
            if (sites.Count == 0)
                return null;
            Console.WriteLine("#\tDepth\tLast update\t\tUrl");
            Console.WriteLine(new string('-', 60));
            foreach (var site in sites)
                Console.WriteLine($"{site.Id}\t{site.Depth}\t{site.LastUpdate}\t{site.Url}");

            return sites;
        }
    }
}