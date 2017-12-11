using System;

// Note that you need to make sure your Project is set to ".NET Framework 4" and NOT ".NET Framework 4 Client Profile"
// Once that is set, make sure the following references are present under the References tree under the project:
// Microsoft.CSharp, System, System.Web.Extensions, and System.XML

namespace Sample_CSharp_API_Client
{
    public class EmailApiPass
    {
        static void Main(string[] args)
        {
            string apiKey = "Your_email_verification_api_key";

            string email = "support@whoisxmlapi.com";
            string url = "https://emailverification.whoisxmlapi.com/api/v1?"
                + "emailAddress=" + email
                + "&apiKey=" + apiKey;

            // Download JSON into a dynamic object
            dynamic result = new System.Net.WebClient().DownloadString(url);

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

    }
}
