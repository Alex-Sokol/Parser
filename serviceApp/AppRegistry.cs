using SiteParser;
using SiteParser.Interfaces;
using StructureMap;

namespace serviceApp
{
    public class AppRegistry : Registry
    {
        public AppRegistry()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
            });

            For<IParseServise>().Use<ParseService>();
            For<IParser>().Singleton().Use<Parser>();
            For<ITreeBuilder>().Use<TreeBuilder>();
            For<IPageManager>().Use<PageManager>();
        }
    }
}