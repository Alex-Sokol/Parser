using DAL.Entities;

namespace SiteParser.Interfaces
{
    public interface ITreeBuilder
    {
        void Build(Site site);
        void Build(string url);
    }
}