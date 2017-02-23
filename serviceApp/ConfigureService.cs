using Topshelf;
namespace serviceApp
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<ParserWinService>(service =>
                {
                    service.ConstructUsing(s => new ParserWinService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("Parser");
                configure.SetDisplayName("Parser");
                configure.SetDescription("My .Net windows service with Topshelf");
            });
        }
    }
}
