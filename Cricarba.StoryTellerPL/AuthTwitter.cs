using Newtonsoft.Json;

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Cricarba.StoryTellerPL
{
    internal static class Twitter
    {
        //internal void Auth(out TwitAuthenticateResponse twitAuthResponse)
        //{
        //    // You need to set your own keys and screen name
        //    var oAuthConsumerKey = "YRHwZvpHlTPynGCGRjQ28l5yg";
        //    var oAuthConsumerSecret = "DTV30wAeMrETIX46le2RIbgYvSWGeXAxx6lI6UGmGMzf1XbrxP";
        //    var oAuthUrl = "https://api.twitter.com/oauth2/token";

        //    // Do the Authenticate
        //    var authHeaderFormat = "Basic {0}";

        //    var authHeader = string.Format(authHeaderFormat,
        //        Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
        //        Uri.EscapeDataString((oAuthConsumerSecret)))
        //    ));

        //    var postBody = "grant_type=client_credentials";

        //    HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
        //    authRequest.Headers.Add("Authorization", authHeader);
        //    authRequest.Method = "POST";
        //    authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
        //    authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        //    using (Stream stream = authRequest.GetRequestStream())
        //    {
        //        byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
        //        stream.Write(content, 0, content.Length);
        //    }

        //    authRequest.Headers.Add("Accept-Encoding", "gzip");

        //    WebResponse authResponse = authRequest.GetResponse();
        //    using (authResponse)
        //    {
        //        using (var reader = new StreamReader(authResponse.GetResponseStream()))
        //        {
        //            var objectText = reader.ReadToEnd();
        //            twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
        //        }
        //    }
        //}

        //internal void NewTweet(TwitAuthenticateResponse twitAuthResponse, string tweet)
        //{
        //    string timelineUrl = $"https://api.twitter.com/1.1/statuses/update.json";
        //    string timelineHeaderFormat = "{0} {1}";
        //    HttpWebRequest a = (HttpWebRequest)WebRequest.Create(timelineUrl);
        //    a.Headers.Add("Authorization", string.Format(timelineHeaderFormat, twitAuthResponse.token_type, twitAuthResponse.access_token));
        //    a.Method = "Post";
        //    a.Method = "Post";
        //    a.ContentType = "application/x-www-form-urlencoded";
        //    using (Stream objStream = a.GetRequestStream())
        //    {
        //        byte[] content = ASCIIEncoding.ASCII.GetBytes("status=" + Uri.EscapeDataString(tweet));
        //        objStream.Write(content, 0, content.Length);
        //    }
        //    WebResponse aResponse = a.GetResponse();
        //    var timeLineJson = string.Empty;

        //    using (aResponse)
        //    {
        //        using (var reader = new StreamReader(aResponse.GetResponseStream()))
        //        {
        //            timeLineJson = reader.ReadToEnd();
        //            //var result = Tweets.FromJson(timeLineJson);
        //        }
        //    }
        //}
        public static void Tweet(string message)
        {
            string twitterURL = "https://api.twitter.com/1.1/statuses/update.json";

            string oauth_consumer_key = "S3pq2lpVO9A34AhVLqwqtKY9e";
            string oauth_consumer_secret = "gImHjaJVsaO9qY7BSCTDH293lz6uHscDSZgILlYj9BhvdS1Hjf";
            string oauth_token = "1234320497179086848-LRb0PjLZu2dhht2IHW2tM8KCYZPkRB";
            string oauth_token_secret = "8q2cDGRMYHHTYlIsbSG2H0BaIAdBA4a1b0OgQgC8C67GQ";

            // set the oauth version and signature method
            string oauth_version = "1.0";
            string oauth_signature_method = "HMAC-SHA1";

            // create unique request details
            string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            System.TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            string oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // create oauth signature
            string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" + "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

            string baseString = string.Format(
                baseFormat,
                oauth_consumer_key,
                oauth_nonce,
                oauth_signature_method,
                oauth_timestamp, oauth_token,
                oauth_version,
                Uri.EscapeDataString(message)
            );

            string oauth_signature = null;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(Uri.EscapeDataString(oauth_consumer_secret) + "&" + Uri.EscapeDataString(oauth_token_secret))))
            {
                oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes("POST&" + Uri.EscapeDataString(twitterURL) + "&" + Uri.EscapeDataString(baseString))));
            }

            // create the request header
            string authorizationFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " + "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " + "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " + "oauth_version=\"{6}\"";

            string authorizationHeader = string.Format(
                authorizationFormat,
                Uri.EscapeDataString(oauth_consumer_key),
                Uri.EscapeDataString(oauth_nonce),
                Uri.EscapeDataString(oauth_signature),
                Uri.EscapeDataString(oauth_signature_method),
                Uri.EscapeDataString(oauth_timestamp),
                Uri.EscapeDataString(oauth_token),
                Uri.EscapeDataString(oauth_version)
            );

            HttpWebRequest objHttpWebRequest = (HttpWebRequest)WebRequest.Create(twitterURL);
            objHttpWebRequest.Headers.Add("Authorization", authorizationHeader);
            objHttpWebRequest.Method = "POST";
            objHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            using (Stream objStream = objHttpWebRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes("status=" + Uri.EscapeDataString(message));
                objStream.Write(content, 0, content.Length);
            }

            var responseResult = "";

            try
            {
                //success posting
                WebResponse objWebResponse = objHttpWebRequest.GetResponse();
                StreamReader objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                responseResult = objStreamReader.ReadToEnd().ToString();
            }
            catch (Exception ex)
            {
                responseResult = "Twitter Post Error: " + ex.Message.ToString() + ", authHeader: " + authorizationHeader;
            }
        }
    }
}