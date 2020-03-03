using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Cricarba.StoryTellerPL
{
    public class StoryTeller
    {
        private IWebDriver driver;

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }

        [SetUp]
        public void startBrowser()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var pathChromeDriver = path.Replace("Cricarba.StoryTellerPL.exe", "");
            driver = new ChromeDriver(pathChromeDriver);
        }

        [Test]
        public void StoryTellerTest()
        {
            int timeMatch = 1;
            while (timeMatch <= 46)
            {
                driver.Url = "https://www.premierleague.com/match/46718";
                Thread.Sleep(5000);
                IWebElement element = driver.FindElement(By.CssSelector(".commentaryContainer"));
                IWebElement tweet = driver.FindElement(By.CssSelector(".tweet"));
                IWebElement hashTag = tweet.FindElement(By.TagName("strong"));

                IWebElement teamHome = driver.FindElement(By.CssSelector(".team.home .teamName .long"));
                IWebElement teamAway = driver.FindElement(By.CssSelector(".team.away .teamName .long"));
                IWebElement score = driver.FindElement(By.CssSelector(".matchScoreContainer .centre .score.fullTime"));
                IReadOnlyCollection<IWebElement> links = element.FindElements(By.TagName("li"));

                if (links.Any())
                {
                    var line = links.Skip(1).Take(1).First();
                    try
                    {
                        IWebElement card = line.FindElement(By.CssSelector(".blogCard"));
                        IWebElement time = card.FindElement(By.CssSelector(".cardMeta time"));
                        IWebElement cardContent = card.FindElement(By.CssSelector(".cardContent"));
                        IWebElement innerContent = cardContent.FindElement(By.CssSelector(".innerContent"));
                        IWebElement type = innerContent.FindElement(By.TagName("h6"));
                        IWebElement text = innerContent.FindElement(By.TagName("p"));

                        string tweetTemplate = string.IsNullOrEmpty(type.Text) ? $"{hashTag.Text} /n /n⚽ {teamHome.Text} {score.Text} {teamAway.Text} /n /n🕕 {time.Text}  /n /n🎙️ {text.Text} /n /n#PremierLeague" :
                                                                                 $"{hashTag.Text} /n /n⚽ {teamHome.Text} {score.Text} {teamAway.Text} /n /n🕕 {time.Text}  /n /n🎙️ {type.Text} {text.Text} /n /n#PremierLeague";
                        tweetTemplate = tweetTemplate.Replace("/n", System.Environment.NewLine);
                        Twitter.Tweet(tweetTemplate);
                        timeMatch++;
                        Thread.Sleep(50000);
                    }
                    catch (System.Exception ex)
                    {
                        Thread.Sleep(50000);
                    }
                }
            }


        }
    }
}