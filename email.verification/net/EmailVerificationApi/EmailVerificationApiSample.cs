using System;
using System.Net;

using Newtonsoft.Json;

// Note that you need to make sure your Project is set to ".NET Framework 4"
// and NOT ".NET Framework 4 Client Profile".

namespace EmailVerificationApi
{
    internal static class EmailVerificationExample
    {
        private static void Main()
        {
            var sample = new EmailVerificationApiSample
            {
                ApiKey = "Your email verification api key"
            };
            
            const string email = "support@whoisxmlapi.com";
            
            // Download JSON
            var result = sample.SendGet(email);

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
    
    public class EmailVerificationApiSample
    {
        public string ApiKey { get; set; }
        
        private const string Url =
            "https://emailverification.whoisxmlapi.com/api/v1";
        
        public string SendGet(string email)
        {
            ///////////////////////////
            // Use the JSON resource //
            ///////////////////////////
            var requestParams = "?emailAddress=" + Uri.EscapeDataString(email)
                              + "&apiKey=" + Uri.EscapeDataString(ApiKey);

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