// <copyright company="Aranda Software">
// © Todos los derechos reservados
// </copyright>

using Newtonsoft.Json;

using System;
using System.IO;
using System.Net;
using System.Text;

namespace Cricarba.StoryTellerPL
{
    internal class AuthTwitter
    {
        internal static void Auth(out TwitAuthenticateResponse twitAuthResponse)
        {
            // You need to set your own keys and screen name
            var oAuthConsumerKey = "b7NKTLy7cdvZ5kasHvjbMjG7u";
            var oAuthConsumerSecret = "z2PgPjPgtwTkXjMsCYajLKdErY03H6gwTNWnQ0OLlZln1lX6iM";
            var oAuthUrl = "https://api.twitter.com/oauth2/token";

            // Do the Authenticate
            var authHeaderFormat = "Basic {0}";

            var authHeader = string.Format(authHeaderFormat,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                Uri.EscapeDataString((oAuthConsumerSecret)))
            ));

            var postBody = "grant_type=client_credentials";

            HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = authRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            authRequest.Headers.Add("Accept-Encoding", "gzip");

            WebResponse authResponse = authRequest.GetResponse();
            using (authResponse)
            {
                using (var reader = new StreamReader(authResponse.GetResponseStream()))
                {
                    var objectText = reader.ReadToEnd();
                    twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
                }
            }
        }

        internal static void NewTweet(TwitAuthenticateResponse twitAuthResponse, out string timeLineJson)
        {
            string timelineUrl = $"https://api.twitter.com/1.1/search/tweets.json?";
            string timelineHeaderFormat = "{0} {1}";
            HttpWebRequest a = (HttpWebRequest)WebRequest.Create(timelineUrl);
            a.Headers.Add("Authorization", string.Format(timelineHeaderFormat, twitAuthResponse.token_type, twitAuthResponse.access_token));
            a.Method = "Get";
            WebResponse aResponse = a.GetResponse();
            timeLineJson = string.Empty;

            using (aResponse)
            {
                using (var reader = new StreamReader(aResponse.GetResponseStream()))
                {
                    timeLineJson = reader.ReadToEnd();
                    //  var result = Tweets.FromJson(timeLineJson);
                }
            }
        }
    }
}