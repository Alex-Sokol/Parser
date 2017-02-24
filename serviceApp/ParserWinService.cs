using System;
using NLog;

namespace serviceApp
{
    public class ParserWinService
    {
        public static Logger Log { get; set; }
        public static Checker Checker { get; set; }

        public void Start()
        {
            try
            {
                Log = LogManager.GetCurrentClassLogger();
                Log.Info("Servise Start");

                Checker = new Checker();
                Checker.Start();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        public void Stop()
        {
            Log.Info("Servise Stop"); 
        }
    }
}