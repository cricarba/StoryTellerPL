using System;
using System.Collections.Generic;
using System.Linq;
using Cricarba.StoryTellerPL.Dto;
using System.Threading;


namespace Cricarba.StoryTellerPL.Core
{
    public static class PremierTeller
    {
        public static void SummaryMatch(int id)
        {
            List<string> previousTweets = new List<string>();
            bool isEndTime = false;
            bool isHalfTime = false;

            PremierLeagueScrapper premierLeague = new PremierLeagueScrapper();
            TwiteerAuth auth = new TwiteerAuth();
            Twitter twitter = new Twitter(auth);
            while (!isEndTime)
            {
                List<TweetST> tweets = premierLeague.GetTweets(id).ToList();
                foreach (var tweetTemplate in tweets.OrderBy(x => x.Time))
                {
                    if (!string.IsNullOrEmpty(tweetTemplate.Template) && !previousTweets.Contains(tweetTemplate.Template))
                    {
                        previousTweets.Add(tweetTemplate.Template);

                        Console.ForegroundColor = ConsoleColor.White;
                        if (tweetTemplate.HasImage)
                            twitter.TweetImage(tweetTemplate.Image, tweetTemplate.Template);
                        else
                            twitter.TweetSimple(tweetTemplate.Template);

                        Console.Write(tweetTemplate.Template);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("\n Tweet Repetido");
                    }

                    if (tweetTemplate.IsEndTime)
                    {
                        isEndTime = true;
                        break;
                    }

                    if (tweetTemplate.IsHalfTime)
                    {
                        isHalfTime = true;
                        break;
                    }
                }
                Thread.Sleep(isHalfTime ? 1200000 : 15000);
            }

            Console.Write("\n Fin Partido");
        }

    }
}
