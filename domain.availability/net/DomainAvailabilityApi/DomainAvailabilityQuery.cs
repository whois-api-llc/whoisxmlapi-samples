using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml;
using System.Web.Script.Serialization;

// Note that you need to make sure your Project is set to ".NET Framework 4"
// and NOT ".NET Framework 4 Client Profile". Once that is set, make sure the
// following references are present under the References tree under the
// project: Microsoft.CSharp, System, System.Web.Extensions, and System.XML.

namespace DomainAvailabilityApi
{
    public static class DomainAvailabilityQuery
    {
        private static void Main()
        {
            //////////////////////////
            // Fill in your details //
            //////////////////////////
            const string apiKey = "Your domain availability api key";
            const string domain = "google.com";

            var apiUrl = "https://domain-availability.whoisxmlapi.com/api/v1"
                       + "?domainName=" + Uri.EscapeDataString(domain)
                       + "&apiKey=" + Uri.EscapeDataString(apiKey);

            /////////////////////////
            // Use a JSON resource //
            /////////////////////////
            var format = "JSON";

            var url = apiUrl
                    + "&outputFormat=" + Uri.EscapeDataString(format);

            // Create our JSON parser
            var jsc = new JavaScriptSerializer();
            jsc.RegisterConverters(new JavaScriptConverter[] {
                new DynamicJsonConverter()
            });

            // Download and parse the JSON into a dynamic object
            dynamic result = jsc.Deserialize(
               new System.Net.WebClient().DownloadString(url),typeof(object));

            // Print a nice informative string
            try
            {
                Console.WriteLine("JSON:\n");
                result.PrintPairs();
            }
            catch (Exception e)
            {
                try
                {
                    Console.WriteLine("JSON:\nErrorMessage:\n\t{0}",
                                      result.ErrorMessage.msg);
                }
                catch (Exception e2)
                {
                    Console.WriteLine("An unkown error has occurred!");
                }
            }

            /////////////////////////
            // Use an XML resource //
            /////////////////////////
            format = "XML";

            url = apiUrl + "&outputFormat=" + Uri.EscapeDataString(format);

            var settings = new XmlReaderSettings();
            var reader = XmlReader.Create(url, settings);

            try
            {
                var domType = typeof(DomainInfo);

                var serializer =
                    new System.Xml.Serialization.XmlSerializer(domType);

                var record = (DomainInfo)serializer.Deserialize(reader);

                reader.Close();

                // Print a nice informative string
                Console.WriteLine("XML:\n");
                record.PrintToConsole();
            }
            catch (Exception e)
            {
                try
                {
                    var errType = typeof(ErrorMessage);
                    var serializer =
                        new System.Xml.Serialization.XmlSerializer(errType);

                    var errorMessage =
                        (ErrorMessage)serializer.Deserialize(reader);

                    reader.Close();

                    // Print a nice informative string
                    Console.WriteLine("XML:\nErrorMessage:\n\t{0}",
                                      errorMessage.msg);
                }
                catch (Exception e2)
                {
                    Console.WriteLine("XML:\nException: {0}", e2.Message);
                }
            }

            // Prevent command window from automatically closing
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        //////////////////
        // JSON Classes //
        //////////////////

        public class DynamicJsonObject : DynamicObject
        {
            private IDictionary<string, object> Dictionary { get; }

            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                Dictionary = dictionary;
            }

            public override bool TryGetMember(
                GetMemberBinder binder,
                out object result
            )
            {
                result = Dictionary[binder.Name];

                if (result is IDictionary<string, object>)
                {
                    result = new DynamicJsonObject(
                                    result as IDictionary<string, object>);
                }
                else if (result is ArrayList
                      && (result as ArrayList) is IDictionary<string, object>)
                {
                    result = new List<DynamicJsonObject>(
                        (result as ArrayList).ToArray().Select(
                            x => new DynamicJsonObject(
                                    x as IDictionary<string, object>)));
                }
                else if (result is ArrayList)
                {
                    result =new List<object>((result as ArrayList).ToArray());
                }

                return Dictionary.ContainsKey(binder.Name);
            }

            public void PrintPairs()
            {
                string s;

                foreach (var pair in Dictionary)
                {
                    try
                    {
                        s = ((string)pair.Value).Replace("\n", "");

                        s = pair.Key + ": "
                          + s.Substring(0, (s.Length < 40 ? s.Length : 40))
                          + "\n";

                        Console.Write(s);
                    }
                    catch (Exception e)
                    {
                        s = pair.Key + ":\n";
                        Console.Write(s);

                        foreach (var subpair in pair.Value
                                 as Dictionary<string, object>)
                        {
                            try
                            {
                                var val = subpair.Value;
                                var valType = val.GetType().ToString();

                                s = valType.Equals("System.Int32")
                                        ? val.ToString()
                                        : ((string)val).Replace("\n", "");

                                var pos = (s.Length < 40 ? s.Length : 40);

                                s = "\t" + subpair.Key + ": "
                                  + s.Substring(0, pos) + "\n";

                                Console.Write(s);
                            }
                            catch (Exception e2)
                            {
                                Console.Write("\t" + subpair.Key + ":\n");

                                try
                                {
                                    foreach (var subsubpair in subpair.Value
                                        as Dictionary<string, object>)
                                    {
                                        var v = subsubpair.Value;
                                        var tp = v.GetType().ToString();

                                        const string tpVal =
                                            "System.Collections.ArrayList";

                                        s = v.ToString().Replace("\n", "");

                                        if (tp.Equals(tpVal)) {
                                           foreach (var value in (ArrayList)v)
                                              Console.Write(
                                                "\t\t" + value + "\n");
                                           continue;
                                        }

                                        var ps=(s.Length <40 ? s.Length : 40);

                                        s = "\t\t" + subsubpair.Key + ": "
                                          + s.Substring(0, ps) + "\n";

                                        Console.Write(s);
                                    }
                                }
                                catch (Exception e3) {
                                    Console.Write(subpair.Value + "\n");
                                }
                            }
                        }
                    }
                }
            }
        }

        public class DynamicJsonConverter : JavaScriptConverter
        {
            public override object Deserialize(
                IDictionary<string, object> dictionary,
                Type type,
                JavaScriptSerializer serializer
            )
            {
                if (dictionary == null)
                    throw new ArgumentNullException(nameof(dictionary));

                if (type == typeof(object))
                {
                    return new DynamicJsonObject(dictionary);
                }

                return null;
            }

            public override IDictionary<string, object> Serialize(
                object obj,
                JavaScriptSerializer serializer
            )
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<Type> SupportedTypes
            {
              get {
                return
                  new System.Collections.ObjectModel.ReadOnlyCollection<Type>(
                    new List<Type>(new Type[] { typeof(object) }));
              }
            }
        }

        /////////////////
        // XML Classes //
        /////////////////

        [Serializable]
        public class ErrorMessage
        {
            [System.Xml.Serialization.XmlElement("msg")]
            public string msg { get; set; }
        }

        [Serializable()]
        public class DomainInfo
        {
            [System.Xml.Serialization.XmlElement("domainName")]
            public string domainName { get; set; }

            [System.Xml.Serialization.XmlElement("domainAvailability")]
            public string domainAvailability { get; set; }

            public void PrintToConsole()
            {
                Console.WriteLine("DomainInfo:");
                Console.WriteLine("\tdomainName " + domainName);

                Console.WriteLine("\tdomainAvailability: "
                                  + domainAvailability);

            }
        }
    }
}
