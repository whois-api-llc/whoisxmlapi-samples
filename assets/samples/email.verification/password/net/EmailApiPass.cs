using System;
using System.Net;
using System.IO;

class Program {
    public const string EMAIL = "support@whoisxmlapi.com";
    public const string PASSWORD = "your whois api password";
    public const string USERNAME = "your whois api username";

    static void Main() {
        string url = "http://www.whoisxmlapi.com/whoisserver/EmailVerifyService?"
                   + $"emailAddress={DOMAIN}&username={USERNAME}"
                   + $"&password={PASSWORD}&checkFree=true&checkDNS=true"
                   + $"&checkSMTP=true&checkCatchAll=true&checkDisposable=true";
        string whoisData = string.Empty;

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream)) {
            whoisData = reader.ReadToEnd();
        }
        Console.WriteLine(whoisData);
    }
}