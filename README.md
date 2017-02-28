# whoisxmlapi-samples

Samples of whoisxmlapi.com API for different languages.

Here you may explore samples on folowing languages:

* [Java](#java)
* [JS](#js)
* [dotNet](#dotnet)
* [Perl](#perl)
* [PHP](#php)
* [Python](#python)
* [Ruby](#ruby)


## Java

[Browse all Java samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/java)

```java
package com.whoisxmlapi;

import java.io.IOException;
import java.io.StringReader;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
import org.w3c.dom.Document;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

/**
 * @author jonz
 */
public class SimpleQuery {
    public static void main(String[] args) {
        String API_URL = "http://www.whoisxmlapi.com/whoisserver/WhoisService";
        String domainName = "test.com";
        String username = null, password = null;

        String url = API_URL + "?domainName=" + domainName;
        if (username != null)
            url += "&userName=" + username + "&password=" + password;

        HttpClient httpclient = null;
        try {
            httpclient = new DefaultHttpClient();
            HttpGet httpget = new HttpGet(url);
            System.out.println("executing request " + httpget.getURI());

            // Create a response handler
            ResponseHandler<String> responseHandler = new BasicResponseHandler();
            String responseBody = httpclient.execute(httpget, responseHandler);
            System.out.println(responseBody);
            System.out.println("----------------------------------------");

            //parse

            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            InputSource is = new InputSource();
            is.setCharacterStream(new StringReader(responseBody));
            Document doc = db.parse(is);

            System.out.println("Root element " + doc.getDocumentElement().getNodeName());

        } catch (SAXException ex) {
            ex.printStackTrace();
        } catch (ParserConfigurationException ex) {
            ex.printStackTrace();
        } catch (IOException ex) {
            ex.printStackTrace();
        } finally {
            if (httpclient != null)
                httpclient.getConnectionManager().shutdown();
        }
    }
}
```

## JS

[Browse all JS samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/js)


```javascript
<!DOCTYPE html>
<html>
<head>
    <title>Sample Javascript API Client</title>
    <script type="text/javascript">
        // Fill in your details
        var username = "YOUR_USERNAME";
        var password = "YOUR_PASSWORD";
        var domain = "google.com";
        var format = "JSON";
        var jsonCallback = "LoadJSON";
        window.addEventListener("load", onPageLoad, false);
        function onPageLoad() {
            // Use a JSON resource
            var url = "http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=" + domain + "&username=" + username + "&password=" + password + "&outputFormat=" + format;
            // Dynamically Add a script to get our JSON data from a different server, avoiding cross origin problems
            var head = document.getElementsByTagName('head')[0];
            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = url + "&callback=" + jsonCallback;
            head.appendChild(script);
            // The function specified in jsonCallback will be called with a single argument representing the JSON object
        }
        // Do something with the json result we get back
        function LoadJSON(result) {
            // Print out a nice informative string
            document.body.innerHTML += "<div>JSON:</div>" + RecursivePrettyPrint(result);
        }
        function RecursivePrettyPrint(obj) {
            var str = "";
            for (var x in obj) {
                if (obj.hasOwnProperty(x)) {
                    str += '<div style="margin-left: 25px;border-left:1px solid black">' + x + ": ";
                    if (typeof(obj[x]) == "string")
                        str += obj[x];
                    else
                        str += RecursivePrettyPrint(obj[x]);
                    str += "</div>";
                }
            }
            return str;
        }
    </script>
</head>
<body>
</body>
</html>
```

## dotNet

[Browse all .Net samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/net)


```c#
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
```

## Perl

[Browse all Perl samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/perl)


```perl
#!/usr/bin/perl

use LWP::Simple;                # From CPAN
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;               # Perl core module
use strict;                     # Good practice
use warnings;                   # Good practice

my $base_url = "https://www.whoisxmlapi.com/whoisserver/WhoisService";
my $domain_name = "google.com";
my $format = "json";
my $user_name = "";
my $password = "";

# 'get' is exported by LWP::Simple;
my $url = "$base_url?domainName=$domain_name&outputFormat=$format";
if ($user_name ne "") {
    $url = "$url&userName=$user_name&password=$password";
}

print "Get data by URL: $url\n";
my $json = get($url);
die "Could not get $base_url!" unless defined $json;

# Decode the entire JSON
my $decoded_json = decode_json($json);

# Dump all data if need
#print Dumper $decoded_json;

# Print fetched attribute
print "Domain Name: ", $decoded_json->{'WhoisRecord'}->{'domainName'}, "\n";
print "Contact Email: ", $decoded_json->{'WhoisRecord'}->{'contactEmail'}, "\n";
```

## PHP

[Browse all PHP samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/php)

```php
<?php
//////////////////////////
// Fill in your details //
//////////////////////////
$username = "YOUR_USERNAME";
$password = "YOUR_PASSWORD";
$domain = "google.com";
$format = "JSON"; //or XML
$url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName='. $domain .'&username='. $username .'&password='. $password .'&outputFormat='. $format;
if($format=='JSON'){
  /////////////////////////
  // Use a JSON resource //
  /////////////////////////
  // Get and build the JSON object
  $result = json_decode(file_get_contents($url));
  // Print out a nice informative string
  print ("<div>JSON:</div>" . RecursivePrettyPrint($result));
}
else{
  ////////////////////////
  // Use a XML resource //
  ////////////////////////
  $url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName='. $domain .'&username='. $username .'&password='. $password .'&outputFormat='. $format;
  // Get and build the XML associative array
  $parser = new XMLtoArray();
  $result = array("WhoisRecord" =>$parser->ParseXML($url));
  // Print out a nice informative string
  print ("<div>XML:</div>" . RecursivePrettyPrint($result));
}
// Function to recursively print all properties of an object and their values
function RecursivePrettyPrint($obj)
{
  $str = "";
  foreach ((array)$obj as $key => $value)
  {
    if (!is_string($key)) // XML parsing leaves a little to be desired as it fills our obj with key/values with just whitespace, ignore that whitespace at the cost of losing hostNames and ips in the final printed result
      continue;
    $str .= '<div style="margin-left: 25px;border-left:1px solid black">' . $key . ": ";
    if (is_string($value))
      $str .= $value;
    else
      $str .= RecursivePrettyPrint($value);
    $str .= "</div>";
  }

  return $str;
}
// Class that simply turns an xml tree into a multilevel associative array
class XMLtoArray
{
  private $root;
  private $stack;
  public function __construct()
  {
    $this->root = null;
    $this->stack = array();
  }

  function ParseXML($feed_url)
  {
    $xml_parser = xml_parser_create();

    xml_parser_set_option($xml_parser, XML_OPTION_CASE_FOLDING, 0);// or throw new Exception('Unable to Set Case Folding option on XML Parser!');
    xml_parser_set_option($xml_parser, XML_OPTION_SKIP_WHITE, 1);// or throw new Exception('Unable to Set Skip Whitespace option on XML Parser!');
    xml_set_object($xml_parser, $this);
    xml_set_element_handler($xml_parser, "startElement", "endElement");
    xml_set_character_data_handler($xml_parser, "characterData");
    $fp = fopen($feed_url,"r");// or throw new Exception("Unable to read URL!");
    while ($data = fread($fp, 4096))
      xml_parse($xml_parser, $data, feof($fp));// or throw new Exception(sprintf("XML error: %s at line %d", xml_error_string(xml_get_error_code($xml_parser)), xml_get_current_line_number($xml_parser)));
    fclose($fp);
    xml_parser_free($xml_parser);

    return $this->root;
  }

  public function startElement($parser, $tagName, $attrs)
  {
    if ($this->root == null)
    {
      $this->root = array();
      $this->stack[] = &$this->root;
    }
    else
    {
      $parent = &$this->stack[count($this->stack)-1];
      if (!is_array($parent))
        $parent = array($parent);
      if (isset($parent[$tagName]))
      {
        if (!is_array($parent[$tagName]))
          $parent[$tagName] = array($parent[$tagName]);
      }
      else
        $parent[$tagName] = null;

      $this->stack[] = &$parent[$tagName];
    }
  }
  public function endElement($parser, $tagName)
  {
    array_pop($this->stack);
  }
  public function characterData($parser, $data)
  {
    $data = trim($data);

    $current = &$this->stack[count($this->stack)-1];
    if (is_array($current))
      $current[] = $data;
    else
      $current = $data;
  }
}
?>
```

## Python

[Browse all Python samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/python)

```python
import urllib.request
import json
import xml.etree.ElementTree as etree

########################
# Fill in your details #
########################
username = "YOUR_USERNAME"
password = "YOUR_PASSWORD"
domain = "google.com"

# A function to recursively print out multi-level dicts with indentation
def RecursivePrettyPrint(obj, indent):
    for x in list(obj):
        if isinstance(obj[x], dict):
            print (' '*indent + str(x)[0:50] + ": ")
            RecursivePrettyPrint(obj[x], indent + 5)
        elif isinstance(obj[x], list):
            print(' '*indent + str(x)[0:50] + ": " + str(list(obj[x])))
        else:
            print (' '*indent + str(x)[0:50] + ": " + str(obj[x])[0:50].replace("\n",""))

#######################
# Use a JSON resource #
#######################
format = "JSON"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format

# Get and build the JSON object
result = json.loads(urllib.request.urlopen(url).readall().decode('utf8'))

# Handle some odd JS cases for audit, whose properties are named '$' and '@class'.  Dispose of '@class' and just make '$' the value for each property
if 'audit' in result:
	if 'createdDate' in result['audit']:
		if '$' in result['audit']['createdDate']:
			result['audit']['createdDate'] = js['audit']['createdDate']['$']
	if 'updatedDate' in result['audit']:
		if '$' in result['audit']['updatedDate']:
			result['audit']['updatedDate'] = js['audit']['updatedDate']['$']

# Get a few data members.
if ('WhoisRecord' in result):
    registrantName = result['WhoisRecord']['registrant']['name']
    domainName = result['WhoisRecord']['domainName']
    createdDate = result['WhoisRecord']['createdDate']

# Print out a nice informative string
RecursivePrettyPrint(result, 0)

#######################
# Use an XML resource #
#######################
format = "XML"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format
result = etree.parse(urllib.request.urlopen(url))
root = result.getroot()

# A function to recursively build a dict out of an ElementTree
def etree_to_dict(t):
    if (len(list(t)) == 0):
        d = t.text
    else:
        d = {}
        for node in list(t):
            d[node.tag] = etree_to_dict(node)
            if isinstance(d[node.tag], dict):
                d[node.tag] = d[node.tag];
    return d

# Create the dict with the above function.
result = {root.tag :etree_to_dict(root)}

# Get a few data members.
if ('WhoisRecord' in result):
    registrantName = result['WhoisRecord']['registrant']['name']
    domainName = result['WhoisRecord']['domainName']
    createdDate = result['WhoisRecord']['createdDate']

# Print out a nice informative string
#print ("'" + registrantName + "' created " + domainName + " on " + createdDate)
RecursivePrettyPrint(result, 0)
```

## Ruby

[Browse all Ruby samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/ruby)

```ruby
require 'open-uri'
require 'json'
require 'rexml/document'
require 'rexml/xpath'
require 'yaml'		# only needed to print the returned result in a very pretty way

########################
# Fill in your details #
########################
username = "YOUR_USERNAME"
password = "YOUR_PASSWORD"
domain = "google.com"

#######################
# Use a JSON resource #
#######################
format = "JSON"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format

# Open the resource
buffer = open(url).read

# Parse the JSON result
result = JSON.parse(buffer)

# Print out a nice informative string
puts "XML:\n" + result.to_yaml + "\n"

#######################
# Use an XML resource #
#######################
format = "XML"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format

# Open the resource
buffer = open(url).read

# Parse the XML result
result = REXML::Document.new(buffer)

# Get a few data members and make sure they aren't nil
if ((errorMessage = REXML::XPath.first(result, "/ErrorMessage/msg")) != nil)
	puts "JSON:\nErrorMessage:\n\t" + errorMessage.text
else
	registrantName = (registrantName = REXML::XPath.first(result, "/WhoisRecord/registrant/name")) == nil ? '' : registrantName.text
	domainName = (domainName = REXML::XPath.first(result, "/WhoisRecord/domainName")) == nil ? '' : domainName.text
	createdDate = (createdDate = REXML::XPath.first(result, "WhoisRecord/createdDate")) == nil ? '' : createdDate.text

	# Print out a nice informative string
	puts "JSON:\n'" + registrantName + "' created " + domainName + " on " + createdDate
end
```
