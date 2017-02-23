using DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class Site
    {
        public Site()
        {
            Pages = new List<Page>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool ExternalLinks { get; set; }
        public int Depth { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        
    }
}