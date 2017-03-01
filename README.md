# Making a query to Whois API web service

Examples of using [www.whoisxmlapi.com](https://www.whoisxmlapi.com/) Hosted Whois Web Service RESTful API
implemented in multiple languages:

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

            ResponseHandler<String> responseHandler = new BasicResponseHandler();
            String responseBody = httpclient.execute(httpget, responseHandler);
            System.out.println(responseBody);

            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            InputSource is = new InputSource();
            is.setCharacterStream(new StringReader(responseBody));
            Document doc = db.parse(is);

            System.out.println(
				"Root element " 
				+ doc.getDocumentElement().getNodeName()
			);

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


```html
<!DOCTYPE html>
<html>
<head>
    <title>JQuery JSONP Sample</title>
    <script
        src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js"></script>
    <script type="text/javascript">
        var username = "YOUR_USERNAME";
        var password = "YOUR_PASSWORD";
        var domain = "google.com";
        $(function () {
            $.ajax({
                url: "http://www.whoisxmlapi.com/whoisserver/WhoisService?callback=?",
                dataType: "jsonp",
                data: {
                    username: username,
                    password: password,
                    domainName: domain,
                    outputFormat: "json"
                },
                success: function (response) {
                    $("#json").append("<div>JSON answer:</div>" + JSON.stringify(response, null, 2));
                },
                error: function(e){
                    console.log(e);
                }
            });
        });
    </script>
</head>
<body>
<pre id="json"></pre>
</body>
</html>
```

## dotNet

[Browse all .Net samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/net)

> Note that you need to make sure your Project is set to ".NET Framework 4" and NOT ".NET Framework 4 Client Profile".
> Once that is set, make sure the following references are present under the References tree under the project: Microsoft.CSharp, System, System.Web.Extensions, and System.XML


```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Dynamic;
using System.Collections;
using System.Web.Script.Serialization;


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
            string url = "http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=" 
						+ domain 
						+ "&username=" + username 
						+ "&password=" + password 
						+ "&outputFormat=" + format;

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

            // Prevent command window from automatically closing during debugging
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

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
$format = "JSON";

$url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?' 
	. 'domainName=' . $domain 
	. '&username=' . $username 
	. '&password='. $password 
	. '&outputFormat='. $format;


$result = json_decode(file_get_contents($url));
print ("<div>JSON:</div>" . RecursivePrettyPrint($result));


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
?>
```

## Python

[Browse all Python samples](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/python)

```python
import urllib.request
import json

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

format = "JSON"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'\
	+ 'domainName=' + domain\
	+ '&username=' + username\
	+ '&password=' + password\
	+ '&outputFormat=' + format

# Get and build the JSON object
result = json.loads(urllib.request.urlopen(url).readall().decode('utf8'))

# Handle some odd JS cases for audit, whose properties are named '$' and '@class'.  
# Dispose of '@class' and just make '$' the value for each property
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

format = "JSON"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'\
	+ 'domainName=' + domain\
	+ '&username=' + username\
	+ '&password=' + password\
	+ '&outputFormat=' + format

# Open the resource
buffer = open(url).read

# Parse the JSON result
result = JSON.parse(buffer)

# Print out a nice informative string
puts "XML:\n" + result.to_yaml + "\n"
```
