using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace Sample_CSharp_API_Client
{
    public class ReverseWhoisClassic
    {
        static void Main(string[] args)
        {
            string username = "your whois api username";
            string password = "your whois api password";
            string term = "wikimedia";
            string format = "JSON";
            string url = 
                "http://www.whoisxmlapi.com/reverse-whois-api/search.php?" 
                + "term1=" + term + "&username=" + username + "&password=" 
                + password + "&outputFormat=" + format + "&mode=preview";

            dynamic result = new System.Net.WebClient().DownloadString(url);

            try
            {
                Console.WriteLine("JSON:\n");
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("An unkown error has occurred!");
            }

        }
    }
}
