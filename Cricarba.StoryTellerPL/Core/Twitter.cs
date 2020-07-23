using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Cricarba.StoryTellerPL.Core
{
    public class Twitter
    {
        private TwiteerAuth _twitAuthenticate;
        public Twitter(TwiteerAuth twitAuthenticate)
        {
            _twitAuthenticate = twitAuthenticate;
        }
        public void TweetImage(string url, string tweetTemplate)
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
                    Auth.SetUserCredentials(_twitAuthenticate.ConsumerKey, _twitAuthenticate.ConsumerSecret, _twitAuthenticate.Token, _twitAuthenticate.TokenSecret);

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

        public void TweetSimple(string tweetTemplate)
        {
            try
            {
                Auth.SetUserCredentials(_twitAuthenticate.ConsumerKey, _twitAuthenticate.ConsumerSecret, _twitAuthenticate.Token, _twitAuthenticate.TokenSecret);
                var tweet = Tweet.PublishTweet(tweetTemplate);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(ex.Message);
            }
        }
    }
}
