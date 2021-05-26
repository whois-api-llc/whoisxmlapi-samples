using System;
using System.Xml;

// Note that you need to make sure your Project is set to ".NET Framework 4"
// and NOT ".NET Framework 4 Client Profile". Once that is set, make sure the
// following references are present under the References tree under the
// project: Microsoft.CSharp, System, System.Web.Extensions, and System.XML.

namespace DnsLookupApi
{
    public static class DnsApiQuery
    {
        private static void Main()
        {
            //////////////////////////
            // Fill in your details //
            //////////////////////////
            const string apiKey = "Your dns lookup api key";
            const string checkType = "_all";
            const string domain = "google.com";

            var apiUrl = "https://www.whoisxmlapi.com/whoisserver/DNSService"
                       + "?type=" + Uri.EscapeDataString(checkType)
                       + "&domainName=" + Uri.EscapeDataString(domain)
                       + "&apiKey=" + Uri.EscapeDataString(apiKey)

            /////////////////////////
            // Use a JSON resource //
            /////////////////////////
            var format = "JSON";
            var url = apiUrl
                    + "&outputFormat=" + Uri.EscapeDataString(format);

            // Download JSON into a dynamic object
            dynamic result = new System.Net.WebClient().DownloadString(url);

            // Print a nice informative string
            try
            {
                Console.WriteLine("JSON:\n");
                Console.WriteLine(result);
            }
            catch (Exception)
            {
                Console.WriteLine("An unkown error has occurred!");
            }

            /////////////////////////
            // Use an XML resource //
            /////////////////////////
            format = "XML";
            url = apiUrl + "&outputFormat=" + Uri.EscapeDataString(format);

            // Download XML into a dynamic object
            result = new System.Net.WebClient().DownloadString(url);
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                WriteXml(xmlDoc);
            }
            catch (Exception)
            {
                try
                {
                    Console.WriteLine("JSON:\nErrorMessage:\n\t{0}",
                                      result.ErrorMessage.msg);
                }
                catch (Exception)
                {
                    Console.WriteLine("An unkown error has occurred!");
                }
            }

            // Prevent command window from automatically closing
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void WriteXml(XmlDocument doc)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            var writer = XmlWriter.Create(Console.Out, settings);

            doc.WriteTo(writer);
            writer.Flush();
            Console.WriteLine();
        }
    }
}
