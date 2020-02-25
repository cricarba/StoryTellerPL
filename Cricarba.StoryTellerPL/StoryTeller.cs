// <copyright company="Aranda Software">
// © Todos los derechos reservados
// </copyright>

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System.Collections.Generic;
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
            driver = new ChromeDriver(@"C:\Users\cristian.carvajal\Downloads\chromedriver71");
        }

        [Test]
        public void StoryTellerTest()
        {
            driver.Url = "https://www.premierleague.com/match/46870";
            Thread.Sleep(5000);
            IWebElement element = driver.FindElement(By.CssSelector(".commentaryContainer"));
            IWebElement tweet = driver.FindElement(By.CssSelector(".tweet"));
            IWebElement hashTag = tweet.FindElement(By.TagName("strong"));

            IWebElement teamHome = driver.FindElement(By.CssSelector(".team.home .teamName .long"));
            IWebElement teamAway = driver.FindElement(By.CssSelector(".team.away .teamName .long"));

            IReadOnlyCollection<IWebElement> links = element.FindElements(By.TagName("li"));

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\cristian.carvajal\Downloads\chromedriver71\WriteLines.txt"))
            {
                foreach (var line in links)
                {
                    try
                    {
                        IWebElement card = line.FindElement(By.CssSelector(".blogCard"));
                        IWebElement time = card.FindElement(By.CssSelector(".cardMeta time"));
                        IWebElement cardContent = card.FindElement(By.CssSelector(".cardContent"));
                        IWebElement innerContent = cardContent.FindElement(By.CssSelector(".innerContent"));
                        IWebElement type = innerContent.FindElement(By.TagName("h6"));
                        IWebElement text = innerContent.FindElement(By.TagName("p"));

                        string tweetTemplate = $"{hashTag.Text} /n /n⚽ {teamHome.Text} - {teamAway.Text} /n /n🕕 {time.Text}  /n /n🎙️ {text.Text} /n /n#PremierLeague";
                        tweetTemplate = tweetTemplate.Replace("/n", System.Environment.NewLine);
                        file.WriteLine(tweetTemplate);
                    }
                    catch (System.Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }
}