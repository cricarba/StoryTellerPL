// <copyright company="Aranda Software">
// © Todos los derechos reservados
// </copyright>

using IronWebScraper;

namespace Cricarba.StoryTellerPL
{
    internal class MatchScrapper : WebScraper
    {
        public override void Init()
        {
            //this.LoggingLevel = WebScraper.LogLevel.Critical;
            //this.Request($"https://www.premierleague.com/match/46858", Parse);
        }

        public override void Parse(Response response)
        {
            //var torneos = response.Css(".commentaryContainer");
            //if (torneos.Any())
            //{
            //    var nombreTorneo = torneos[0].ChildNodes[2].InnerTextClean;
            //}
        }
    }
}