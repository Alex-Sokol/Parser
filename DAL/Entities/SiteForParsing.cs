using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SiteForParsing
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Depth { get; set; }
        public int NumberOfThreads { get; set; }
        public bool ExternalLinks { get; set; }
        public bool Tree { get; set; }
    }
}
