using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cricarba.StoryTellerPL
{
    class tweeteat
    {
		public void UpdateStatus(string message)
		{
			// Application tokens
			const string CONSUMER_KEY = "YOUR_CONSUMER_KEY";
			const string CONSUMER_SECRET = "YOUR_CONSUMER_SECRET";
			// Access tokens
			const string ACCESS_TOKEN = "YOUR_ACCESS_TOKEN";
			const string ACCESS_TOKEN_SECRET = "YOUR_ACCESS_TOKEN_SECRET";

			// Common parameters
			const string VERSION = "1.0";
			const string SIGNATURE_METHOD = "HMAC-SHA1";

			// Parameters specific to this request
			var nonce = new Random().Next(0x0000000, 0x7fffffff).ToString("X8");
			var timestamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

			// Prepare the text to sign
			var dataToSign = new StringBuilder();
			
			dataToSign
				.Append("POST")
				.Append("&")
				.Append("http://api.twitter.com/1/statuses/update.xml".EncodeRFC3986())
				.Append("&");

			// Values to sign: oauth parameters AND request parameters
			var signatureParts = new Dictionary<string, string>
	{
		{"status", message.EncodeRFC3986()},
		{"oauth_version", VERSION},
		{"oauth_consumer_key", CONSUMER_KEY},
		{"oauth_nonce", nonce},
		{"oauth_signature_method", SIGNATURE_METHOD},
		{"oauth_timestamp", timestamp},
		{"oauth_token", ACCESS_TOKEN}
	};

			dataToSign.Append(
				signatureParts
					.OrderBy(x => x.Key)
					.Select(x => "{0}={1}".FormatWith(x.Key, x.Value))
					.Join("&")
					.EncodeRFC3986());

			// Calculate the signature key required to sign the request
			var signatureKey = "{0}&{1}".FormatWith(CONSUMER_SECRET.EncodeRFC3986(), ACCESS_TOKEN_SECRET.EncodeRFC3986());
			var sha1 = new HMACSHA1(Encoding.ASCII.GetBytes(signatureKey));

			var signatureBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(dataToSign.ToString()));
			var signature = Convert.ToBase64String(signatureBytes);

			// Create and setup the actual request
			var request = (HttpWebRequest)WebRequest.Create("http://api.twitter.com/1/statuses/update.xml");
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";

			// Set authorization header
			request.Headers.Add(
				"Authorization",
				new StringBuilder("OAuth ")
					.AppendFormat("{0}=\"{1}\"", "oauth_nonce".EncodeRFC3986(), nonce.EncodeRFC3986()).Append(",")
					.AppendFormat("{0}=\"{1}\"", "oauth_signature_method".EncodeRFC3986(), SIGNATURE_METHOD.EncodeRFC3986()).Append(",")
					.AppendFormat("{0}=\"{1}\"", "oauth_timestamp".EncodeRFC3986(), timestamp.EncodeRFC3986()).Append(",")
					.AppendFormat("{0}=\"{1}\"", "oauth_consumer_key".EncodeRFC3986(), CONSUMER_KEY.EncodeRFC3986()).Append(",")
					.AppendFormat("{0}=\"{1}\"", "oauth_token".EncodeRFC3986(), ACCESS_TOKEN.EncodeRFC3986()).Append(",")
					.AppendFormat("{0}=\"{1}\"", "oauth_signature".EncodeRFC3986(), signature.EncodeRFC3986()).Append(",")
					.AppendFormat("{0}=\"{1}\"", "oauth_version".EncodeRFC3986(), VERSION.EncodeRFC3986())
					.ToString());

			// Add request body with request parameters
			var requestBody = Encoding.ASCII.GetBytes("status={0}".FormatWith(message.EncodeRFC3986()));
			using (var stream = request.GetRequestStream())
				stream.Write(requestBody, 0, requestBody.Length);

			// ... and here we go!
			request.GetResponse();

			// We could analyze the response, but since we'll get an exception if 
			// something fails, we don't need to mess with response content
		}

		public static string Join<T>(this IEnumerable<T> items, string separator)
		{
			return string.Join(separator, items.ToArray());
		}

		public static string FormatWith(this string format, params object[] args)
		{
			return string.Format(format, args);
		}

		// From Twitterizer http://www.twitterizer.net/
		public static string EncodeRFC3986(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			var encoded = Uri.EscapeDataString(value);

			return Regex
				.Replace(encoded, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper())
				.Replace("(", "%28")
				.Replace(")", "%29")
				.Replace("$", "%24")
				.Replace("!", "%21")
				.Replace("*", "%2A")
				.Replace("'", "%27")
				.Replace("%7E", "~");
		}
	}
}
