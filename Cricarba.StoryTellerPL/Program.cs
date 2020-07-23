using Cricarba.StoryTellerPL.Core;
using Cricarba.StoryTellerPL.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Cricarba.StoryTellerPL
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Id del partido:");
            var numero = Console.ReadLine();
            int matchId = int.Parse(numero);
            SummaryMatch(matchId);
        }

        private static void SummaryMatch(int id)
        {
            List<string> previousTweets = new List<string>();
            bool isEndTime = false;
            bool isHalfTime = false;

            PremierLeague premierLeague = new PremierLeague();
            TwiteerAuth twitAuthenticate = new TwiteerAuth();
            Twitter twitter = new Twitter(twitAuthenticate);
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

                        Console.WriteLine(tweetTemplate.Template);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Tweet Repetido");
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
                Thread.Sleep(isHalfTime ? 1020000 : 30000);
            }

            Console.WriteLine("Fin Partido");
        }



    }
}