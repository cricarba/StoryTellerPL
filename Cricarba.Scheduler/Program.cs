using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cricarba.StoryTellerPL.Core;
using Hangfire;

namespace Cricarba.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration
                  .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseColouredConsoleLogProvider()
                  .UseSimpleAssemblyNameTypeSerializer().UseSqlServerStorage(@"Data Source=CRICARBA;Initial Catalog=Hangfire;User ID=hangfire;Password=13beer88rojo;Application Name=MyApp");
            Shedule();


        }


        private static void Shedule()
        {
            //MatchSchedule matchSchedule = new MatchSchedule();

            ///RecurringJob.AddOrUpdate(() => matchSchedule.ScheduleMatch(), Cron.Daily(22, 10));
            bool other = true;
            while (other)
            {
                Console.WriteLine("Partido:");
                var id = Console.ReadLine();
                Console.WriteLine("Horas:");
                var horas = Console.ReadLine();
                int.TryParse(id, out int idMatch);
                double.TryParse(horas, out double time);
                double timeToMatch = time * 60;
                Console.WriteLine($"Agendadndo para {timeToMatch} minutos");
                BackgroundJob.Schedule(() => PremierTeller.SummaryMatch(idMatch), TimeSpan.FromMinutes(timeToMatch));
                Console.WriteLine("Agendado");
                Console.WriteLine("Otro S/N");
                other = Console.ReadLine() == "S";

            }
        }
    }
}
