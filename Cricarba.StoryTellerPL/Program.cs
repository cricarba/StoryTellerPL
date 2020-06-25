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
            while (!isEndTime)
            {
                List<TweetST> tweets = premierLeague.GetTweets(id).ToList();
                foreach (var tweetTemplate in tweets.OrderByDescending(x => x.Time))
                {
                    if (!string.IsNullOrEmpty(tweetTemplate.Template) && !previousTweets.Contains(tweetTemplate.Template))
                    {
                        previousTweets.Add(tweetTemplate.Template);

                        Console.ForegroundColor = ConsoleColor.White;
                        if (tweetTemplate.HasImage) 
                            TweetImage(tweetTemplate.Image, tweetTemplate.Template);
                        else
                            Twitter.Tweet(tweetTemplate.Template);

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
                Thread.Sleep(isHalfTime ? 900000 : 15000);
            }

            Console.Write("\n Fin Partido");
        }

      
        private static void TweetImage(string url, string tweetTemplate)
        {
            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;
                System.Net.WebResponse webResponse = webRequest.GetResponse();
                System.IO.Stream stream = webResponse.GetResponseStream();
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    byte[] file1 = ms.ToArray();
                    Auth.SetUserCredentials("S3pq2lpVO9A34AhVLqwqtKY9e", "gImHjaJVsaO9qY7BSCTDH293lz6uHscDSZgILlYj9BhvdS1Hjf", "1234320497179086848-LRb0PjLZu2dhht2IHW2tM8KCYZPkRB", "8q2cDGRMYHHTYlIsbSG2H0BaIAdBA4a1b0OgQgC8C67GQ");

                    var media = Upload.UploadBinary(file1);

                    var tweet = Tweet.PublishTweet(tweetTemplate, new PublishTweetOptionalParameters
                    {
                        Medias = new List<IMedia> { media }
                    });

                }
                webResponse.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(ex.Message);
            }
        }
    }
}