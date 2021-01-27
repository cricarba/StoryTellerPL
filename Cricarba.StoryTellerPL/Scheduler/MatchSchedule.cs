using System;
using System.Collections.Generic;
using System.Threading;
using Cricarba.StoryTellerPL.Core;
using Hangfire;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Cricarba.StoryTellerPL.Scheduler
{
    public class MatchSchedule
    {
        public void ScheduleMatch()
        {
            IWebDriver driver;
            var chromeDriver = new Secrets().GetSecrects("chromeDriver");
            driver = new ChromeDriver(chromeDriver);
            driver.Url = $"https://www.premierleague.com/fixtures";
            Thread.Sleep(5000);
            IWebElement element = driver.FindElement(By.CssSelector(".fixtures"));
            IReadOnlyCollection<IWebElement> matchDays = driver.FindElements(By.CssSelector(".fixtures__matches-list"));

            foreach (var day in matchDays)
            {
                var date = day.GetAttribute("data-competition-matches-list");
                DateTime dateMatch = Convert.ToDateTime(date);
                IWebElement matcList = day.FindElement(By.CssSelector(".matchList"));
                IReadOnlyCollection<IWebElement> list = matcList.FindElements(By.TagName("li"));
                foreach (var match in list)
                {
                    var id = match.GetAttribute("data-comp-match-item");
                    int.TryParse(id, out int identifier);

                    IWebElement time = match.FindElement(By.TagName("time"));
                    var splitTime = time.Text.Split(':');
                    if (splitTime.Length > 1)
                    {
                        int.TryParse(splitTime[0], out int hour);
                        int.TryParse(splitTime[1], out int minute);
                        dateMatch = dateMatch.AddHours(hour);
                        dateMatch = dateMatch.AddMinutes(minute);
                        double minutesDiff = (dateMatch - DateTime.Now).TotalMinutes;
                        Console.WriteLine($"{time.Text}  ");
                        Console.WriteLine($"{id} datematch {dateMatch} now {DateTime.Now} diff {minutesDiff} ");

                        BackgroundJob.Schedule(() => PremierTeller.SummaryMatch(identifier),
                                 TimeSpan.FromMinutes(minutesDiff));
                    }
                }
            }
        }
    }
}