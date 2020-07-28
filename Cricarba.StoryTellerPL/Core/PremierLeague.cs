using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cricarba.StoryTellerPL.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Cricarba.StoryTellerPL.Core
{
    internal class PremierLeague
    {
        List<string> _previousPhotos = new List<string>();
        public IEnumerable<TweetST> GetTweets(int matchId)
        {
            List<TweetST> template = new List<TweetST>();
            IWebDriver driver;
            var chromeDriver = @"C:\Users\Freddy Castelblanco\Documents\Archivos\Proyectos\StoryTellerPL\Cricarba.StoryTellerPL\";
            driver = new ChromeDriver(chromeDriver);
            try
            {
                driver.Url = $"https://www.premierleague.com/match/{matchId}";
                Thread.Sleep(5000);
                IWebElement element = driver.FindElement(By.CssSelector(".commentaryContainer"));
                IReadOnlyCollection<IWebElement> links = element.FindElements(By.TagName("li"));

                List<string> photos = GetPhotoMatch(driver);
                if (links.Any())
                {
                    int take = links.Count > 3 ? 3 : 1;
                    var lines = links.Take(take);
                    foreach (var item in lines)
                    {
                        var newTweet = CreateTemplate(item, driver);
                        foreach (var photo in photos)
                        {
                            if (!_previousPhotos.Contains(photo))
                            {
                                newTweet.HasImage = true;
                                newTweet.Image = photo;
                                _previousPhotos.Add(photo);
                                break;
                            }
                        }
                        template.Add(newTweet);
                    }
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

        private static TweetST CreateTemplate(IWebElement line, IWebDriver driver)
        {
            string timeMatch = "--";
            TweetST newTweet = new TweetST();
            IWebElement tweet = driver.FindElement(By.CssSelector(".tweet"));
            IWebElement hashTag = tweet.FindElement(By.TagName("strong"));
            IWebElement teamHome = driver.FindElement(By.CssSelector(".team.home .teamName .long"));
            IWebElement teamAway = driver.FindElement(By.CssSelector(".team.away .teamName .long"));
            IWebElement score = driver.FindElement(By.CssSelector(".matchScoreContainer .centre .score"));
            IWebElement card = line.FindElement(By.CssSelector(".blogCard"));

            try
            {
                IWebElement time = card.FindElement(By.CssSelector(".cardMeta time"));
                timeMatch = time.Text;
                newTweet.Time = GetTime(timeMatch);
            }
            catch (Exception)
            {
            }

            IWebElement cardContent = card.FindElement(By.CssSelector(".cardContent"));
            IWebElement innerContent = cardContent.FindElement(By.CssSelector(".innerContent"));
            IWebElement type = innerContent.FindElement(By.TagName("h6"));
            IWebElement text = innerContent.FindElement(By.TagName("p"));

            string tweetTemplate = string.IsNullOrEmpty(type.Text) ?
                                   $"{hashTag.Text} /n /n⚽ {teamHome.Text} {score.Text} {teamAway.Text} /n /n🕕 {timeMatch}  /n /n🎙️ {text.Text} /n /n#PremierLeague #PL" :
                                   $"{hashTag.Text} /n /n⚽ {teamHome.Text} {score.Text} {teamAway.Text} /n /n🕕 {timeMatch}  /n /n🎙️ {type.Text} {text.Text} /n /n#PremierLeague #PL";

            newTweet.Template = tweetTemplate.Replace("/n", Environment.NewLine);
            return newTweet;
        }

        private static int GetTime(string timeMatch)
        {
            int time;
            timeMatch = timeMatch.Replace("'", string.Empty);
            var timeSplit = timeMatch.Split('+');
            if (timeSplit.Length == 2)
            {
                int.TryParse(timeSplit[0], out int normalTime);
                int.TryParse(timeSplit[1], out int extraTime);
                time = normalTime + extraTime;
            }
            else
                int.TryParse(timeMatch, out time);

            return time;
        }

        private static List<string> GetPhotoMatch(IWebDriver driver)
        {
            List<string> urls = new List<string>();
            try
            {
                bool staleElement = true;
                while (staleElement)
                {
                    try
                    {
                        IWebElement element = driver.FindElement(By.CssSelector(".matchPhotoContainer"));
                        IReadOnlyCollection<IWebElement> gallery = element.FindElements(By.TagName("li"));

                        if (gallery.Any())
                        {
                            foreach (var photo in gallery)
                            {
                                urls.Add(photo.GetAttribute("data-ui-src"));
                            }
                        }
                        staleElement = false;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        staleElement = !urls.Any();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(ex.Message);
                driver.Close();
            }
            return urls;
        }
    }
}

