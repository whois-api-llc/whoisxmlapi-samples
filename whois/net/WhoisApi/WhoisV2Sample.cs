using System;
using System.Net;

using Newtonsoft.Json;

// Note that you need to make sure your Project is set to ".NET Framework 4"
// and NOT ".NET Framework 4 Client Profile".

namespace WhoisApi
{
    internal static class WhoisV2Sample
    {
        private static void Main()
        {
            var sample = new WhoisApiV2Client
            {
                ApiKey = "Your Whois API key"
            };
            
            const string domain = "whoisxmlapi.com";
            
            // Download JSON
            var result = sample.SendGet(domain);

            // Print a nice informative string
            PrintResponse(result);
        }
        
        private static void PrintResponse(string response)
        {
            dynamic responseObject = JsonConvert.DeserializeObject(response);

            if (responseObject != null)
            {
                Console.Write(responseObject);
                Console.WriteLine("--------------------------------");
                return;
            }

            Console.WriteLine();
        }
    }
    
    public class WhoisApiV2Client
    {
        public string ApiKey { get; set; }
        
        private const string Url =
            "https://www.whoisxmlapi.com/whoisserver/WhoisService";
        
        public string SendGet(string domain)
        {
            ///////////////////////////
            // Use the JSON resource //
            ///////////////////////////
            var requestParams = "?domainName=" + Uri.EscapeDataString(domain)
                              + "&apiKey=" + Uri.EscapeDataString(ApiKey)
                              + "&outputFormat=JSON";

            var fullUrl = Url + requestParams;

            Console.Write("Sending request to: " + fullUrl + "\n");

            // Download JSON into a string
            var result = new WebClient().DownloadString(fullUrl);
                
            try
            {
                return result;
            }
            catch (Exception)
            {
                return "{\"error\":\"An unkown error has occurred!\"}";
            }
        }
    }
}