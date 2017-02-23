using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace SiteParser.Interfaces
{
    public interface ITreeBuilder
    {
        void Build(Site site);
        void Build(string url);
    }
}
