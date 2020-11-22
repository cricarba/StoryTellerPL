using Cricarba.StoryTellerPL.Scheduler;

namespace Cricarba.StoryTellerPL
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConfigSchedule schedule = new ConfigSchedule();
            schedule.RunServer();
        }
    }
}