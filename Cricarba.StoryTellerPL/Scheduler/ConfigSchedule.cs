using System;
using Cricarba.StoryTellerPL.Core;
using Hangfire;

namespace Cricarba.StoryTellerPL.Scheduler
{
    public class ConfigSchedule
    {
        public ConfigSchedule()
        {
            Secrets secrets = new Secrets();
            string conn = secrets.GetSecrects("hangFireDb");
            GlobalConfiguration.Configuration
                   .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                   .UseColouredConsoleLogProvider()
                   .UseSimpleAssemblyNameTypeSerializer().UseSqlServerStorage(conn);
        }

        public void RunServer()
        {
            using (var server = new BackgroundJobServer())
            {
                Console.ReadLine();
            }
        }
    }
}