using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace SiteParser.Interfaces
{
    public interface IParser
    {
        Site MainSite { get; set; }
        Task[] Tasks { get; set; }
        void Start(Site site, int numberOfThreads);
    }
}
