﻿using System;
using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Note that you need to make sure your Project is set to ".NET Framework 4"
// and NOT ".NET Framework 4 Client Profile". Once that is set, make sure the
// following references are present under the References tree under the
// project: Microsoft.CSharp, System, System.Web.Extensions, and System.XML.

namespace RegistrantAlertApi
{
    public static class RegistrantAlertApiV2Sample
    {
        private const string ApiKey = "Your registrant alert api key";

        private const string Url =
            "https://registrant-alert.whoisxmlapi.com/api/v2";

        private const string SearchParamsAdvanced =
            @"{
                advancedSearchTerms: [
                    {
                        field: 'RegistrantContact.Name',
                        term: 'Test'
                    }
                ],
                sinceDate: '2018-07-12',
                mode: 'purchase',
                apiKey: 'API_KEY'
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
                mode: 'purchase',
                apiKey: 'API_KEY'
            }";

        private static void Main()
        {
            var responsePost = SendPostRegistrantAlert();
            PrintResponse(responsePost);

            responsePost = SendPostRegistrantAlert(true);
            PrintResponse(responsePost);

            // Prevent command window from automatically closing
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

        private static string SendPostRegistrantAlert(bool isAdvanced=false)
        {
            Console.Write("Sending request to: " + Url + "\n");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var searchParams =
                isAdvanced ? SearchParamsAdvanced : SearchParamsBasic;

            using (var streamWriter =
                        new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = searchParams.Replace("API_KEY", ApiKey);
                var jsonData = JObject.Parse(json).ToString();

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
