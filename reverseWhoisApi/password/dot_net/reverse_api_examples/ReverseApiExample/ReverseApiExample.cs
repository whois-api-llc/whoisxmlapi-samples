using System;
using System.Xml;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Xml;

// Note that you need to make sure your Project is set to ".NET Framework 4" and NOT ".NET Framework 4 Client Profile"
// Once that is set, make sure the following references are present under the References tree under the project:
// Microsoft.CSharp, System, System.Web.Extensions, and System.XML

namespace Sample_CSharp_API_Client
{
    public class ReverseApiExample
    {
        protected string username;
        protected string password;
        public const string url =  "https://www.whoisxmlapi.com/reverse-whois-api/search.php";

        static void Main(string[] args)
        {
            ReverseApiExample reverseWhois = new ReverseApiExample();

            //////////////////////////
            // Fill in your details //
            //////////////////////////
            reverseWhois.setUsername("username");
            reverseWhois.setPassword("xxxxxxxxxx");

            //////////////////////////
            //   send POST request  //
            //////////////////////////
            string responsePost = reverseWhois.sendPostReverseWhois();
            reverseWhois.PrintResponse(responsePost);

            //////////////////////////
            //   send GET request   //
            //////////////////////////
            string responseGet = reverseWhois.sendGetReverseWhois();
            reverseWhois.PrintResponse(responseGet);

            // Prevent command window from automatically closing during debugging
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        protected string sendPostReverseWhois()
        {
            /////////////////////////
            // Use a JSON resource //
            /////////////////////////

            Console.Write("Sending request to: " + url + "\n");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{" +
                    "\"terms\": [" +
                        "{" +
                            "\"section\":\"Registrant\",\"attribute\":\"name\",\"value\":\"brett\",\"matchType\":\"anywhere\",\"exclude\":\"false\"" +
                        "}]," +
                    "\"recordsCounter\":\"false\",\"outputFormat\":\"json\"," +
                    "\"username\":\"" + this.getUsername() + "\"," +
                    "\"password\":\"" + this.getPassword() + "\"," +
                    "\"rows\":\"10\",\"searchType\": \"current\"" +
                    "}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string res;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                res = streamReader.ReadToEnd();
            }
            return res;
        }

        protected string sendGetReverseWhois()
        {
            /////////////////////////
            // Use a JSON resource //
            /////////////////////////
            string requestParams = "?term1=" + "topcoder" +  "&mode=purchase" + "&username=" + this.getUsername() + "&password=" + this.getPassword();
            string fullUrl = url + requestParams;

            Console.Write("Sending request to: " + fullUrl + "\n");

            // Download JSON into a string
            string result = new WebClient().DownloadString(fullUrl);
                
            // Print a nice informative string
            try
            {
                return result;
            }
            catch (Exception e)
            {
                return "{\"error\":\"An unkown error has occurred!\"}";
            }
        }

        public void setUsername(string login)
        {
            this.username = login;
        }
        public void setPassword(string pass)
        {
            this.password = pass;
        }

        public String getUsername()
        {
            return this.username;
        }

        public String getPassword()
        {
            return this.password;
        }

        protected void PrintResponse(string response)
        {
            dynamic responseObject = JsonConvert.DeserializeObject(response);
            if (responseObject != null)
            {
                Console.Write(responseObject);
                Console.WriteLine("--------------------------------");
                return;
            }
            else Console.WriteLine(response);
            Console.WriteLine();
        }

    }

}

