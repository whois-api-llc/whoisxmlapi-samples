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
    public class whois
    {
        static void Main(string[] args)
        {
            //////////////////////////
            // Fill in your details //
            //////////////////////////
            string username = "YOUR_USERNAME";
            string password = "YOUR_PASSWORD";
            string domain = "google.com";

            /////////////////////////
            // Use a JSON resource //
            /////////////////////////
            string format = "JSON";
            string url = "http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=" + domain + "&username=" + username + "&password=" + password + "&outputFormat=" + format;
            
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
            url = "http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=" + domain + "&username=" + username + "&password=" + password + "&outputFormat=" + format;
            
            var settings = new XmlReaderSettings();
            var reader = XmlReader.Create(url, settings);
            WhoisRecord record = new WhoisRecord();
            
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(WhoisRecord));
                record = (WhoisRecord)serializer.Deserialize(reader);

                reader.Close();

                // Print a nice informative string
                Console.WriteLine("XML:");
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
                                s = ((string)subpair.Value).Replace("\n", "");
                                s = "\t" + subpair.Key + ": " + s.Substring(0, (s.Length < 40 ? s.Length : 40)) + "\n";
                                Console.Write(s);
                            }
                            catch (Exception e2)
                            {
                                Console.Write("\t" + subpair.Key + ":\n");

                                foreach (var subsubpair in subpair.Value as System.Collections.Generic.Dictionary<string, object>)
                                {
                                    s = subsubpair.Value.ToString().Replace("\n", "");
                                    s = "\t\t" + subsubpair.Key + ": " + s.Substring(0, (s.Length < 40 ? s.Length : 40)) + "\n";
                                    Console.Write(s);
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
        public class WhoisRecord
        {
            [System.Xml.Serialization.XmlElement("createdDate")]
            public string createdDate { get; set; }
            [System.Xml.Serialization.XmlElement("updatedDate")]
            public string updatedDate { get; set; }
            [System.Xml.Serialization.XmlElement("expiresDate")]
            public string expiresDate { get; set; }
            [System.Xml.Serialization.XmlElement("registrant")]
            public WhoisRecordContact registrant { get; set; }
            [System.Xml.Serialization.XmlElement("administrativeContact")]
            public WhoisRecordContact administrativeContact { get; set; }
            [System.Xml.Serialization.XmlElement("billingContact")]
            public WhoisRecordContact billingContact { get; set; }
            [System.Xml.Serialization.XmlElement("technicalContact")]
            public WhoisRecordContact technicalContact { get; set; }
            [System.Xml.Serialization.XmlElement("zoneContact")]
            public WhoisRecordContact zoneContact { get; set; }
            [System.Xml.Serialization.XmlElement("domainName")]
            public string domainName { get; set; }
            [System.Xml.Serialization.XmlElement("nameServers")]
            public WhoisRecordNameServers nameServers { get; set; }
            [System.Xml.Serialization.XmlElement("rawText")]
            public string rawText { get; set; }
            [System.Xml.Serialization.XmlElement("header")]
            public string header { get; set; }
            [System.Xml.Serialization.XmlElement("strippedText")]
            public string strippedText { get; set; }
            [System.Xml.Serialization.XmlElement("footer")]
            public string footer { get; set; }
            [System.Xml.Serialization.XmlElement("audit")]
            public WhoisRecordAudit audit { get; set; }
            [System.Xml.Serialization.XmlElement("registrarName")]
            public string registrarName { get; set; }
            [System.Xml.Serialization.XmlElement("registryData")]
            public WhoisRecordRegistryData registryData { get; set; }
            [System.Xml.Serialization.XmlElement("domainAvailability")]
            public string domainAvailability { get; set; }
            [System.Xml.Serialization.XmlElement("contactEmail")]
            public string contactEmail { get; set; }
            [System.Xml.Serialization.XmlElement("domainNameExt")]
            public string domainNameExt { get; set; }

            public void PrintToConsole()
            {
                Console.WriteLine("WhoisRecord:");
                Console.WriteLine("\tcreatedDate: " + createdDate);
                Console.WriteLine("\texpdatedDate: " + updatedDate);
                Console.WriteLine("\texpiresDate: " + expiresDate);
                Console.WriteLine("\tregistrant:");
                registrant.PrintToConsole();
                Console.WriteLine("\tadministrativeContact:");
                administrativeContact.PrintToConsole();
                Console.WriteLine("\tbillingContact:");
                billingContact.PrintToConsole();
                Console.WriteLine("\ttechnicalContact:");
                technicalContact.PrintToConsole();
                Console.WriteLine("\ttechnicalContact:");
                technicalContact.PrintToConsole();
                Console.WriteLine("\tzoneContact:");
                zoneContact.PrintToConsole();
                Console.WriteLine("\tdomainName: " + domainName);
                Console.WriteLine("\tnameServers:");
                nameServers.PrintToConsole();
                Console.WriteLine("\trawText: " + rawText.Substring(0, rawText.Length < 40 ? rawText.Length : 40).Replace("\n", ""));
                Console.WriteLine("\theader: " + header.Substring(0, header.Length < 40 ? header.Length : 40).Replace("\n", ""));
                Console.WriteLine("\tstrippedText: " + strippedText.Substring(0, strippedText.Length < 40 ? strippedText.Length : 40).Replace("\n", ""));
                Console.WriteLine("\tfooter: " + footer.Substring(0, footer.Length < 40 ? footer.Length : 40).Replace("\n", ""));
                Console.WriteLine("\taudit:");
                audit.PrintToConsole();
                Console.WriteLine("\tregistrarName: " + registrarName);
                Console.WriteLine("\tregistryData:");
                registryData.PrintToConsole();
                Console.WriteLine("\tdomainAvailability: " + domainAvailability);
                Console.WriteLine("\tcontactEmail: " + contactEmail);
                Console.WriteLine("\tdomainNameExt: " + domainNameExt);
            }
        }

        [Serializable()]
        public class WhoisRecordContact
        {
            [System.Xml.Serialization.XmlElement("name")]
            public string name { get; set; }
            [System.Xml.Serialization.XmlElement("organization")]
            public string organization { get; set; }
            [System.Xml.Serialization.XmlElement("street1")]
            public string street1 { get; set; }
            [System.Xml.Serialization.XmlElement("street2")]
            public string street2 { get; set; }
            [System.Xml.Serialization.XmlElement("city")]
            public string city { get; set; }
            [System.Xml.Serialization.XmlElement("state")]
            public string state { get; set; }
            [System.Xml.Serialization.XmlElement("postalCode")]
            public string postalCode { get; set; }
            [System.Xml.Serialization.XmlElement("country")]
            public string country { get; set; }
            [System.Xml.Serialization.XmlElement("email")]
            public string email { get; set; }
            [System.Xml.Serialization.XmlElement("telephone")]
            public string telephone { get; set; }
            [System.Xml.Serialization.XmlElement("rawText")]
            public string rawText { get; set; }
            [System.Xml.Serialization.XmlElement("unparsable")]
            public string unparsable { get; set; }

            public void PrintToConsole()
            {
                Console.WriteLine("\t\t\tname: " + name);
                Console.WriteLine("\t\t\torganization: " + organization);
                Console.WriteLine("\t\t\tstreet1: " + street1);
                Console.WriteLine("\t\t\tstreet2: " + street2);
                Console.WriteLine("\t\t\tcity: " + city);
                Console.WriteLine("\t\t\tstate: " + state);
                Console.WriteLine("\t\t\tpostalCode: " + postalCode);
                Console.WriteLine("\t\t\tcountry: " + country);
                Console.WriteLine("\t\t\temail: " + email);
                Console.WriteLine("\t\t\ttelephone: " + telephone);
                Console.WriteLine("\t\t\trawText: " + rawText);
                Console.WriteLine("\t\t\tunparsable: " + unparsable);
            }
        }

        [Serializable()]
        public class WhoisRecordNameServers
        {
            [System.Xml.Serialization.XmlElement("rawText")]
            public string rawText { get; set; }
            [System.Xml.Serialization.XmlElement("Address")]
            public List<string> hostNames { get; set; }
            [System.Xml.Serialization.XmlElement("class")]
            public List<string> ips { get; set; }

            public void PrintToConsole()
            {
                Console.WriteLine("\t\trawText: " + rawText.Substring(0, (rawText.Length < 40 ? rawText.Length : 40)).Replace("\n", ""));
                Console.WriteLine("\t\thostNames:");
                foreach (string hostname in hostNames)
                    Console.WriteLine("\t\t\t" + hostname);
                Console.WriteLine("\t\tips:\n");
                foreach (string ip in ips)
                    Console.WriteLine("\t\t\t" + ip);
            }
        }

        [Serializable()]
        public class WhoisRecordAudit
        {
            [System.Xml.Serialization.XmlElement("createdDate")]
            public string createdDate { get; set; }
            [System.Xml.Serialization.XmlElement("updatedDate")]
            public string updatedDate { get; set; }

            public void PrintToConsole()
            {
                Console.WriteLine("\t\tcreatedDate: " + createdDate);
                Console.WriteLine("\t\tupdatedDate: " + updatedDate);
            }
        }

        [Serializable()]
        public class WhoisRecordRegistryData
        {
            [System.Xml.Serialization.XmlElement("createdDate")]
            public string createdDate { get; set; }
            [System.Xml.Serialization.XmlElement("updatedDate")]
            public string updatedDate { get; set; }
            [System.Xml.Serialization.XmlElement("expiresDate")]
            public string expiresDate { get; set; }
            [System.Xml.Serialization.XmlElement("registrant")]
            public WhoisRecordContact registrant { get; set; }
            [System.Xml.Serialization.XmlElement("administrativeContact")]
            public WhoisRecordContact administrativeContact { get; set; }
            [System.Xml.Serialization.XmlElement("billingContact")]
            public WhoisRecordContact billingContact { get; set; }
            [System.Xml.Serialization.XmlElement("technicalContact")]
            public WhoisRecordContact technicalContact { get; set; }
            [System.Xml.Serialization.XmlElement("zoneContact")]
            public WhoisRecordContact zoneContact { get; set; }
            [System.Xml.Serialization.XmlElement("domainName")]
            public string domainName { get; set; }
            [System.Xml.Serialization.XmlElement("nameServers")]
            public WhoisRecordNameServers nameServers { get; set; }
            [System.Xml.Serialization.XmlElement("status")]
            public string status { get; set; }
            [System.Xml.Serialization.XmlElement("rawText")]
            public string rawText { get; set; }
            [System.Xml.Serialization.XmlElement("header")]
            public string header { get; set; }
            [System.Xml.Serialization.XmlElement("strippedText")]
            public string strippedText { get; set; }
            [System.Xml.Serialization.XmlElement("footer")]
            public string footer { get; set; }
            [System.Xml.Serialization.XmlElement("audit")]
            public WhoisRecordAudit audit { get; set; }
            [System.Xml.Serialization.XmlElement("registrarName")]
            public string registrarName { get; set; }
            [System.Xml.Serialization.XmlElement("whoisServer")]
            public string whoisServer { get; set; }
            [System.Xml.Serialization.XmlElement("referralURL")]
            public string referralURL { get; set; }
            [System.Xml.Serialization.XmlElement("createdDateNormalized")]
            public string createdDateNormalized { get; set; }
            [System.Xml.Serialization.XmlElement("updatedDateNormalized")]
            public string updatedDateNormalized { get; set; }
            [System.Xml.Serialization.XmlElement("expiresDateNormalized")]
            public string expiresDateNormalized { get; set; }

            public void PrintToConsole()
            {
                Console.WriteLine("\t\tcreatedDate: " + createdDate);
                Console.WriteLine("\t\tupdatedDate: " + updatedDate);
                Console.WriteLine("\t\texpiresDate: " + expiresDate);
                Console.WriteLine("\t\tregistrant:");
                registrant.PrintToConsole();
                Console.WriteLine("\t\tadministrativeContact:");
                administrativeContact.PrintToConsole();
                Console.WriteLine("\t\tbillingContact:");
                billingContact.PrintToConsole();
                Console.WriteLine("\t\ttechnicalContact:");
                technicalContact.PrintToConsole();
                Console.WriteLine("\t\tzoneContact:");
                zoneContact.PrintToConsole();
                Console.WriteLine("\t\tdomainName: " + domainName);
                Console.WriteLine("\t\tnameServers:");
                nameServers.PrintToConsole();
                Console.WriteLine("\t\tstatus: " + status.Substring(0, (status.Length < 40 ? status.Length : 40)).Replace("\n", ""));
                Console.WriteLine("\t\trawText: " + rawText.Substring(0, (rawText.Length < 40 ? rawText.Length : 40)).Replace("\n", ""));
                Console.WriteLine("\t\theader: " + header.Substring(0, (header.Length < 40 ? header.Length : 40)).Replace("\n", ""));
                Console.WriteLine("\t\tstrippedText: " + strippedText.Substring(0, (strippedText.Length < 40 ? strippedText.Length : 40)).Replace("\n", ""));
                Console.WriteLine("\t\tfooter: " + footer.Substring(0, (footer.Length < 40 ? footer.Length : 40)).Replace("\n", ""));
                Console.WriteLine("\t\taudit:");
                audit.PrintToConsole();            
                Console.WriteLine("\t\tregistrarName: " + registrarName);
                Console.WriteLine("\t\twhoisServer: " + whoisServer);
                Console.WriteLine("\t\treferralURL: " + referralURL);
                Console.WriteLine("\t\tcreatedDateNormalized: " + createdDateNormalized);
                Console.WriteLine("\t\tupdatedDateNormalized: " + updatedDateNormalized);
                Console.WriteLine("\t\texpiresDateNormalized: " + expiresDateNormalized);
            }
        }
    } 
}