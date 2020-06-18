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
            Console.Write("Numero del partido:");
            var numero = Console.ReadLine();
            int matchId = int.Parse(numero);
            Console.Write("1.Paritdo \n2.Imagen \n");
            var opcion = Console.ReadLine();
            if (opcion == "1")
                SummaryMatch(matchId);
            else
                PhotoMatch(matchId);
        }

        private static void SummaryMatch(int id)
        {
            int timeMatch = 0;
            List<string> previousTweets = new List<string>();
            while (timeMatch <= 46)
            {                
                string tweetTemplate = GetTempalte(id);
                if (!string.IsNullOrEmpty(tweetTemplate) && !previousTweets.Contains(tweetTemplate))
                {
                    previousTweets.Add(tweetTemplate);
                    Console.ForegroundColor = ConsoleColor.White;
                    Twitter.Tweet(tweetTemplate);
                    Console.Write(tweetTemplate);
                    timeMatch++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("\n Tweet Repetido");
                     
                }
                
                Console.Write($"\n {timeMatch}");
                Thread.Sleep(15000);
            }
        }
        private static string GetTempalte(int numero)
        {

            string template = string.Empty;
            IWebDriver driver;

            var chromeDriver = @"C:\Users\Freddy Castelblanco\Documents\Archivos\Proyectos\StoryTellerPL\Cricarba.StoryTellerPL\";
            driver = new ChromeDriver(chromeDriver);
            try
            {
                driver.Url = $"https://www.premierleague.com/match/{numero}";
                Thread.Sleep(5000);
                IWebElement element = driver.FindElement(By.CssSelector(".commentaryContainer"));
                IWebElement tweet = driver.FindElement(By.CssSelector(".tweet"));
                IWebElement hashTag = tweet.FindElement(By.TagName("strong"));
                IWebElement teamHome = driver.FindElement(By.CssSelector(".team.home .teamName .long"));
                IWebElement teamAway = driver.FindElement(By.CssSelector(".team.away .teamName .long"));
                IWebElement score = driver.FindElement(By.CssSelector(".matchScoreContainer .centre .score"));
                IReadOnlyCollection<IWebElement> links = element.FindElements(By.TagName("li"));
                if (links.Any())
                {
                    var line = links.First();
                    template = CreateTemplate(line, hashTag, teamHome, teamAway, score);
                    driver.Close();
                }
            }
            catch (Exception ex)
            {
                driver.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(ex.Message);
            }
            return template;
        }

        private static void PhotoMatch(int numero)
        {
            var chromeDriver = @"C:\Users\Freddy Castelblanco\Documents\Archivos\Proyectos\StoryTellerPL\Cricarba.StoryTellerPL\";
            IWebDriver driver;
            driver = new ChromeDriver(chromeDriver);
            try
            {
                driver.Url = $"https://www.premierleague.com/match/{numero}";
                Thread.Sleep(10000);
                bool staleElement = true;
                while (staleElement)
                {
                    try
                    {
                        IWebElement element = driver.FindElement(By.CssSelector(".matchPhotoContainer"));
                        IReadOnlyCollection<IWebElement> gallery = element.FindElements(By.TagName("li"));

                        if (gallery.Any())
                        {
                            var photo = gallery.First();
                            var url = photo.GetAttribute("data-ui-src");
                            string tweetTemplate = GetTempalte(numero);
                            TweetImage(url, tweetTemplate);
                            driver.Close();
                            Thread.Sleep(30000);
                        }
                        staleElement = false;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        staleElement = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(ex.Message);
                driver.Close();
                Thread.Sleep(30000);
            }

        }
        private static string CreateTemplate(IWebElement line, IWebElement hashTag, IWebElement teamHome, IWebElement teamAway, IWebElement score)
        {
            string timeMatch = string.Empty;
            IWebElement card = line.FindElement(By.CssSelector(".blogCard"));
            try
            {
                IWebElement time = card.FindElement(By.CssSelector(".cardMeta time"));
                timeMatch = time.Text;
            }
            catch (Exception)
            {
            }

            IWebElement cardContent = card.FindElement(By.CssSelector(".cardContent"));
            IWebElement innerContent = cardContent.FindElement(By.CssSelector(".innerContent"));
            IWebElement type = innerContent.FindElement(By.TagName("h6"));
            IWebElement text = innerContent.FindElement(By.TagName("p"));

            
            string tweetTemplate = string.IsNullOrEmpty(type.Text) ? $"{hashTag.Text} /n /n⚽ {teamHome.Text} {score.Text} {teamAway.Text} /n /n🕕 {timeMatch}  /n /n🎙️ {text.Text} /n /n#PremierLeague #PL" :
                                                               $"{hashTag.Text} /n /n⚽ {teamHome.Text} {score.Text} {teamAway.Text} /n /n🕕 {timeMatch}  /n /n🎙️ {type.Text} {text.Text} /n /n#PremierLeague #PL";
            tweetTemplate = tweetTemplate.Replace("/n", Environment.NewLine);
            return tweetTemplate;


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