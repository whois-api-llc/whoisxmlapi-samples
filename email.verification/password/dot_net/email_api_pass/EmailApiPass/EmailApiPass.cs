using System;
using System.Xml;

// Note that you need to make sure your Project is set to ".NET Framework 4" and NOT ".NET Framework 4 Client Profile"
// Once that is set, make sure the following references are present under the References tree under the project:
// Microsoft.CSharp, System, System.Web.Extensions, and System.XML

namespace Sample_CSharp_API_Client
{
    public class EmailApiPass
    {
        static void Main(string[] args)
        {
            //////////////////////////
            // Fill in your details //
            //////////////////////////
            string username = "username";
            string password = "xxxxxxxxxx";

            string email = "support@whoisxmlapi.com";
            string url = "http://www.whoisxmlapi.com/whoisserver/EmailVerifyService?"
                + "emailAddress=" + email
                + "&validateDNS=true&validateSMTP=true&checkCatchAll=true"
                + "&checkFree=true&checkDisposable=true"
                + "&username=" + username + "&password=" + password;

            /////////////////////////
            // Use an XML resource //
            /////////////////////////

            // default outputFormat is "xml"
            
            // Download JSON into a dynamic object
            dynamic result = new System.Net.WebClient().DownloadString(url);
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                WriteXml(xmlDoc);
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

            /////////////////////////
            // Use a JSON resource //
            /////////////////////////
            string format = "JSON";
            url += "&outputFormat=" + format;

            // Download JSON into a dynamic object
            result = new System.Net.WebClient().DownloadString(url);

            // Print a nice informative string
            try
            {
                Console.WriteLine("JSON:\n");
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("An unkown error has occurred!");
            }
        }

        public static void WriteXml(XmlDocument doc)
        {
            XmlTextWriter writer = new XmlTextWriter(Console.Out);
            writer.Formatting = Formatting.Indented;
            doc.WriteTo(writer);
            writer.Flush();
            Console.WriteLine();
        }
    }
}