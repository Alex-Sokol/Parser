using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace serviceApp
{
    class Program
    {
        static void Main()
        {
            ConfigureService.Configure();
        }
    }
}
