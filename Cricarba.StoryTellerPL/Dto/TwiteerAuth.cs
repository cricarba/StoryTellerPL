using Cricarba.StoryTellerPL.Core;

namespace Cricarba.StoryTellerPL
{
    public class TwiteerAuth
    {
        public string ConsumerKey { get; private set; }

        public string ConsumerSecret { get; private set; }

        public string Token { get; private set; }

        public string TokenSecret { get; private set; }

        public TwiteerAuth()
        {
            var secret = new Secrets();
            ConsumerKey = secret.GetSecrects("oauthConsumerKey");
            ConsumerSecret = secret.GetSecrects("oauthConsumerSecret");
            Token = secret.GetSecrects("oauthToken");
            TokenSecret = secret.GetSecrects("oauthTokenSecret");
        }
    }
}