using System.Threading.Tasks;
using DAL.Entities;

namespace SiteParser.Interfaces
{
    public interface IParser
    {
        Site MainSite { get; set; }
        Task[] Tasks { get; set; }
        void Start(Site site, int numberOfThreads);
    }
}