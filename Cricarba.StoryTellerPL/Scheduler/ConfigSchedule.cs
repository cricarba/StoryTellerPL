using System;
using Hangfire;

namespace Cricarba.StoryTellerPL.Scheduler
{
    public class ConfigSchedule
    {
        public ConfigSchedule()
        {

            GlobalConfiguration.Configuration
                   .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                   .UseColouredConsoleLogProvider()
                   .UseSimpleAssemblyNameTypeSerializer().UseSqlServerStorage(@"Data Source=CRICARBA;Initial Catalog=Hangfire;User ID=hangfire;Password=13beer88rojo;Application Name=MyApp");

        }

        public void RunServer() {
            using (var server = new BackgroundJobServer())
            {

                Console.ReadLine();
            }
        }
    }
}
