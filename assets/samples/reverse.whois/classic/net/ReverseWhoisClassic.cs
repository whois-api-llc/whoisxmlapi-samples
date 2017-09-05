using System;

public class Program
{
    static void Main(string[] args)
    {
        string username = "your whois api username";
        string password = "your whois api password";
        string term = "wikimedia";
        string url = "http://www.whoisxmlapi.com/reverse-whois-api/search.php?"
            + "term1=" + term + "&username=" + username + "&password="
            + password + "&outputFormat=JSON" + "&mode=preview";

        dynamic result = new System.Net.WebClient().DownloadString(url);
        try {
            Console.WriteLine(result);
        }
        catch (Exception e) { }
    }
}