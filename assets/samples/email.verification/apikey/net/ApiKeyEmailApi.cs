using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

class Program {
    public const string email = "support@whoisxmlapi.com";
    public const string key = "your whois api key";
    public const string secret = "your whois api secret key";
    public const string username = "your whois api username";

    static void Main() {
        long time = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))
                            .TotalMilliseconds);
        string reqData='\u007B'+$"\"t\":{time},\"u\":\"{username}\""+'\u007D';
        var reqBytes = Encoding.UTF8.GetBytes(reqData);
        string req = Convert.ToBase64String(reqBytes);

        string data = username + time + key;
        HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
        string hex = BitConverter.ToString(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(data)));
        string digest = hex.Replace("-", "").ToLower();

        string url="http://www.whoisxmlapi.com/whoisserver/EmailVerifyService?"
                  +$"requestObject={req}&digest={digest}&emailAddress={email}";
        string whoisData = string.Empty;

        HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);

        using (HttpWebResponse response = (HttpWebResponse)rq.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream)) {
            whoisData = reader.ReadToEnd();
        }
        Console.WriteLine(whoisData);
    }
}