
using System;
using System.Threading.Tasks;
using Cricarba.StoryTellerPL.Scheduler;
using Hangfire;


namespace Cricarba.StoryTellerPL
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConfigSchedule schedule = new ConfigSchedule();
            MatchSchedule matchSchedule = new MatchSchedule();
            
            RecurringJob.AddOrUpdate(() => matchSchedule.ScheduleMatch(), Cron.Daily(22, 10));
            schedule.RunServer();
        }




    }
}