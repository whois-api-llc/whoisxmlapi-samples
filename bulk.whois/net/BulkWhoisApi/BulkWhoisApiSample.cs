using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Note that you need to make sure your Project is set to ".NET Framework 4"
// and NOT ".NET Framework 4 Client Profile". Once that is set, make sure the
// following references are present under the References tree under the
// project: Microsoft.CSharp, System, System.Web.Extensions, and System.XML.

namespace BulkWhoisApi
{
    public static class BulkWhoisApiSample
    {
        private static void Main()
        {
            //////////////////////////
            // Fill in your details //
            //////////////////////////
            var apiClient = new BulkWhoisApi
            {
                apiKey = "Your bulk whois api key",
                Url="https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices"
            };

            string[] domains = {"google.com", "whoisxmlapi.com"};

            Console.WriteLine("Requesting bulk processing...");
            var requestId = apiClient.CreateRequest(domains);

            Console.WriteLine($"    request ID: {requestId}");

            Console.WriteLine("Waiting for processing to finish...");

            while (true)
            {
                var recordsLeft = apiClient.RecordsLeft(requestId);

                Console.WriteLine($"    records left: {recordsLeft}");

                if (recordsLeft == 0)
                    break;

                Thread.Sleep(3000);
            }

            Console.WriteLine("Downloading results...");

            var records = apiClient.GetRecords(requestId);

            PrintResponse(records);
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

    public class BulkWhoisApi
    {
        public string Url { get; set; }

        public string apiKey { get; set; }

        public string CreateRequest(IEnumerable<string> domains)
        {
            const string template = @"{
                domains: [
                    USER_DOMAINS
                ]
            }";

            var domStr =
                string.Join(",", domains.ToArray().Select(x => $"'{x}'"));

            var dataStr = template.Replace("USER_DOMAINS", domStr);

            var response = JObject.Parse(Post("/bulkWhois", dataStr));
            var httpCode = (int) response["messageCode"];

            if (httpCode != (int) HttpStatusCode.OK)
                throw new Exception((string) response["message"]);

            return (string) response["requestId"];
        }

        public string GetRecords(string id, int start=1, int max=10)
        {
            var jsonStr = $@"{{
                requestId: '{id}',
                startIndex: {start},
                maxRecords: {max}
            }}";

            return Post("/getRecords", jsonStr);
        }

        public int RecordsLeft(string id)
        {
            var response = GetRecords(id, 1, 0);

            return (int) JObject.Parse(response)["recordsLeft"];
        }

        private string Post(string path, string data)
        {
            var baseData = $@"{{
                apiKey: '{apiKey}',
                outputFormat: 'json'
            }}";

            var baseJson = JObject.Parse(baseData);

            baseJson.Merge(JObject.Parse(data));

            return RestClient.SendPostJson(Url + path, baseJson.ToString());
        }
    }

    public static class RestClient
    {
        public static string SendPostJson(string url, string data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter =
                new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var res = "";

            using (var responseStream = httpResponse.GetResponseStream())
            {
                if (responseStream == null || responseStream == Stream.Null)
                {
                    return res;
                }

                using (var streamReader = new StreamReader(responseStream))
                {
                    res = streamReader.ReadToEnd();
                }
            }

            return res;
        }
    }
}
