using System;
using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Note that you need to make sure your Project is set to ".NET Framework 4"
// and NOT ".NET Framework 4 Client Profile". Once that is set, make sure the
// following references are present under the References tree under the
// project: Microsoft.CSharp, System, System.Web.Extensions, and System.XML.

namespace ReverseWhoisApi
{
    public static class ReverseWhoisApiV2HeaderAuthSample
    {
        private const string ApiKey = "Your reverse whois api key";

        private const string Url =
            "https://reverse-whois-api.whoisxmlapi.com/api/v2";
        
        private const string SearchParamsAdvanced =
            @"{
                advancedSearchTerms: [
                    {
                        field: 'RegistrantContact.Name',
                        term: 'Test'
                    }
                ],
                sinceDate: '2018-07-12',
                mode: 'purchase'
            }";

        private const string SearchParamsBasic =
            @"{
                basicSearchTerms: {
                    include: [
                        'test',
                        'US'
                    ],
                    exclude: [
                        'Europe',
                        'EU'
                    ]
                },
                sinceDate: '2018-07-12',
                mode: 'purchase'
            }";

        private static void Main()
        {
            var responsePost = SendPostReverseWhois();
            PrintResponse(responsePost);
            
            responsePost = SendPostReverseWhois(true);
            PrintResponse(responsePost);

            // Prevent command window from closing automatically
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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

            Console.WriteLine(response);
            Console.WriteLine();
        }

        private static string SendPostReverseWhois(bool isAdvanced=false)
        {
            Console.Write("Sending request to: " + Url + "\n");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers["X-Authentication-Token"] = ApiKey;

            var searchParams =
                isAdvanced ? SearchParamsAdvanced : SearchParamsBasic;

            using (var streamWriter =
                        new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var jsonData = JObject.Parse(searchParams).ToString();

                streamWriter.Write(jsonData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var res = "";
            
            using (var response=(HttpWebResponse)httpWebRequest.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null || responseStream == Stream.Null)
                {
                    return res;
                }

                using (var streamReader = new StreamReader(responseStream))
                {
                    res = streamReader.ReadToEnd();
                }

                return res;
            }
        }
    }
}