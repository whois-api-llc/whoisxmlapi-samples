using System;
using System.IO;
using System.Net;
using System.Text;

namespace ReverseWhoisApi
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            HttpWebRequest request =
                (HttpWebRequest) WebRequest.Create(
                    "https://www.whoisxmlapi.com/reverse-whois-api/search.php"
                    );
            request.Method = "POST";
            byte[] data = Encoding.ASCII.GetBytes(
                "{\"terms\": [{\"section\": \"Registrant\", \"attribute\": "
                + "\"Email\", \"value\": \"support@whoisxmlapi.com\", "
                + "\"matchType\": \"exact\", \"exclude\": \"false\"}], "
                + "\"recordsCounter\": \"false\", \"outputFormat\": \"json\","
                + " \"username\": \"your whois api username\", \"password\": "
                + "\"your whois api password\", \"rows\": \"10\", "
                + "\"searchType\": \"current\"\n}");
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length); stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(new StreamReader(
                response.GetResponseStream()).ReadToEnd());
        }
    }
}