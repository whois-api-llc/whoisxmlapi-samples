using System;
using System.Xml;

public class brandAlertApiQuery
{
    static void Main(string[] args)
    {
        string username = "Your whois api username";
        string password = "Your whois api password";
        string term1 = "cinema";
        string exclude_term1 = "online";
        String format = "JSON";
        String url = "https://www.whoisxmlapi.com/brand-alert-api/"
            + "search.php?" + "username=" + username + "&password="
            + password + "&output_format=" + format + "&term1="
            + term1 + "&exclude_term1=" + exclude_term1;
        dynamic result = new System.Net.WebClient().DownloadString(url);
        try {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            WriteXml(xmlDoc);
        }
        catch (Exception e) {
            Console.WriteLine("An error has occurred!");
        }
    }
    public static void WriteXml(XmlDocument doc) {
        XmlTextWriter writer = new XmlTextWriter(Console.Out);
        writer.Formatting = System.Xml.Formatting.Indented;
        doc.WriteTo(writer);
        writer.Flush();
        Console.WriteLine();
    }
}