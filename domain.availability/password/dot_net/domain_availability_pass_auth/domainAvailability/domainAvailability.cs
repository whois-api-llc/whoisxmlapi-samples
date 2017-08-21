using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Dynamic;
using System.Collections;
using System.Web.Script.Serialization;

// Note that you need to make sure your Project is set to ".NET Framework 4" and NOT ".NET Framework 4 Client Profile"
// Once that is set, make sure the following references are present under the References tree under the project:
// Microsoft.CSharp, System, System.Web.Extensions, and System.XML

namespace Sample_CSharp_API_Client
{
    public class domainAvailability
    {
        static void Main(string[] args)
        {
            //////////////////////////
            // Fill in your details //
            //////////////////////////
            string username = "username";
            string password = "xxxxxxxxxx";
            string domain = "google.com";

            /////////////////////////
            // Use a JSON resource //
            /////////////////////////
            string format = "JSON";
            string url = "http://www.whoisxmlapi.com/whoisserver/WhoisService?cmd=GET_DN_AVAILABILITY" + "&domainName=" + domain + "&username=" + username + "&password=" + password + "&outputFormat=" + format;
            
            // Create our JSON parser
            JavaScriptSerializer jsc = new JavaScriptSerializer();
            jsc.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });        

            // Download and parse the JSON into a dynamic object
            dynamic result = jsc.Deserialize(new System.Net.WebClient().DownloadString(url), typeof(object)) as dynamic;

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
                    Console.WriteLine("JSON:\nErrorMessage:\n\t{0}", result.ErrorMessage.msg);
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
            url = "http://www.whoisxmlapi.com/whoisserver/WhoisService?cmd=GET_DN_AVAILABILITY" + "&domainName=" + domain + "&username=" + username + "&password=" + password + "&outputFormat=" + format;
            
            var settings = new XmlReaderSettings();
            var reader = XmlReader.Create(url, settings);
            DomainInfo record = new DomainInfo();
            
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DomainInfo));
                record = (DomainInfo)serializer.Deserialize(reader);

                reader.Close();

                // Print a nice informative string
                Console.WriteLine("XML:\n");
                record.PrintToConsole();
            }
            catch (Exception e)
            {
                try
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ErrorMessage));
                    ErrorMessage errorMessage = (ErrorMessage)serializer.Deserialize(reader);

                    reader.Close();

                    // Print a nice informative string
                    Console.WriteLine("XML:\nErrorMessage:\n\t{0}", errorMessage.msg);
                }
                catch (Exception e2)
                {
                    Console.WriteLine("XML:\nException: {0}", e2.Message);
                }
            }

            // Prevent command window from automatically closing during debugging
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        //////////////////
        // JSON Classes //
        //////////////////

        public class DynamicJsonObject : DynamicObject
        {
            private IDictionary<string, object> Dictionary { get; set; }

            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                this.Dictionary = dictionary;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = this.Dictionary[binder.Name];

                if (result is IDictionary<string, object>)
                {
                    result = new DynamicJsonObject(result as IDictionary<string, object>);
                }
                else if (result is ArrayList && (result as ArrayList) is IDictionary<string, object>)
                {
                    result = new List<DynamicJsonObject>((result as ArrayList).ToArray().Select(x => new DynamicJsonObject(x as IDictionary<string, object>)));
                }
                else if (result is ArrayList)
                {
                    result = new List<object>((result as ArrayList).ToArray());
                }

                return this.Dictionary.ContainsKey(binder.Name);
            }

            public void PrintPairs()
            {
                string s;
                foreach (var pair in this.Dictionary)
                {
                    try
                    {
                        s = ((string)pair.Value).Replace("\n", "");
                        s = pair.Key + ": " + s.Substring(0, (s.Length < 40 ? s.Length : 40)) + "\n";
                        Console.Write(s);
                    }
                    catch (Exception e)
                    {
                        s = pair.Key + ":\n";
                        Console.Write(s);

                        foreach (var subpair in pair.Value as System.Collections.Generic.Dictionary<string, object>)
                        {
                            try
                            {
                                string valType = subpair.Value.GetType().ToString();
                                s = valType.Equals("System.Int32")
                                        ? subpair.Value.ToString()
                                        : ((string)subpair.Value).Replace("\n", "");
                                s = "\t" + subpair.Key + ": " + s.Substring(0, (s.Length < 40 ? s.Length : 40)) + "\n";
                                Console.Write(s);
                            }
                            catch (Exception e2)
                            {
                                Console.Write("\t" + subpair.Key + ":\n");

                                try
                                {
                                    foreach (var subsubpair in subpair.Value as System.Collections.Generic.Dictionary<string, object>)
                                    {
                                        s = subsubpair.Value.ToString().Replace("\n", "");
                                        if (subsubpair.Value.GetType().ToString().Equals("System.Collections.ArrayList")) {
                                            foreach (var value in (ArrayList)subsubpair.Value)
                                                Console.Write("\t\t" + value.ToString() + "\n");
                                            continue;
                                        }
                                        s = "\t\t" + subsubpair.Key + ": " + s.Substring(0, (s.Length < 40 ? s.Length : 40)) + "\n";
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
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                if (dictionary == null)
                    throw new ArgumentNullException("dictionary");

                if (type == typeof(object))
                {
                    return new DynamicJsonObject(dictionary);
                }

                return null;
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<Type> SupportedTypes
            {
                get { return new System.Collections.ObjectModel.ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(object) })); }
            }
        }

        /////////////////
        // XML Classes //
        /////////////////

        [Serializable()]
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
                Console.WriteLine("\tdomainAvailability: " + domainAvailability);
                
            }
        }
    } 
}