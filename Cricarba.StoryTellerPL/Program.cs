using System;

namespace Cricarba.StoryTellerPL
{
    internal class Program
    {
        private static void Main(string[] args)
        {
        }

        private static void RunWebScrappy()
        {
            var match = new MatchScrapper();
            match.RateLimitPerHost = TimeSpan.FromSeconds(5);
            match.Start();
            Console.Read();
        }
    }
}