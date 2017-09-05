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
    public class dnsApiPass
    {
        static void Main(string[] args)
        {
            string username = "Your whois api username";
            string password = "Your whois api password";
            string domain = "google.com";
            string format = "JSON";
            string url = 
                "http://www.whoisxmlapi.com/whoisserver/DNSService?type=_all" 
                + "&domainName=" + domain + "&username=" + username + "&password=" 
                + password + "&outputFormat=" + format;

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
