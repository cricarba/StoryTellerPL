using System;
using Cricarba.StoryTellerPL.Core;
using Hangfire;

namespace Cricarba.Scheduler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var secrets = new Secrets();
            string conn = secrets.GetSecrects("hangFireDb");

            GlobalConfiguration.Configuration
                  .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseColouredConsoleLogProvider()
                  .UseSimpleAssemblyNameTypeSerializer().UseSqlServerStorage(conn);
            Shedule();
        }

        private static void Shedule()
        {
            bool other = true;
            while (other)
            {
                Console.WriteLine("Partido:");
                var id = Console.ReadLine();
                int.TryParse(id, out int idMatch);

                Console.WriteLine("Horas que faltan para el incio del partido:");
                var horas = Console.ReadLine();
                int.TryParse(horas, out int hour);
                hour = hour * 60;

                Console.WriteLine("Minutos que faltan para el incio del partido:");
                var minutos = Console.ReadLine();
                double.TryParse(minutos, out double timeToMatch);
                timeToMatch = timeToMatch + hour;


                Console.WriteLine($"Agendando para {timeToMatch} minutos...");
                BackgroundJob.Schedule(() => PremierTeller.SummaryMatch(idMatch), TimeSpan.FromMinutes(timeToMatch));
                Console.WriteLine("Agendado...");
                Console.WriteLine("Agendar Otro S/N?");
                other = Console.ReadLine().ToLower() == "s";
            }
        }
    }
}