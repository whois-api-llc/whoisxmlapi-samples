using System;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json;

/*
 * Target platform: .Net Framework 4.0
 * 
 * You need to install Newtonsoft JSON.NET
 *
 */


namespace Sample_CSharp_API_Client
{
    class ApiKeyEmailApi
    {
        static void Main(string[] args)
        {
            new ApiKeyEmailApi().Run();
        }

        public void Run()
        {
            string username = "username";
            string apiKey = "api_key";
            string secretKey = "secret_key";

            string url = "https://whoisxmlapi.com/whoisserver/EmailVerifyService?";
            string email = "support@whoisxmlapi.com";
            string type = "_all";
            this.PerformRequest(username, apiKey, secretKey, url, email, type);
        }

        protected void PerformRequest(string username, string apiKey,
            string secretKey, string url, string email, string type)
        {
            long timestamp = this.GetTimeStamp();
            string digest = this.GenerateDigest(
                username, apiKey, secretKey, timestamp
            );
            try
            {
                string request = this.BuildRequest(
                    username, timestamp, digest, email, type
                );
                string response = this.getEmailVerifyRecord(url + request);
                if (response.IndexOf("Request timeout") > -1)
                {
                    timestamp = this.GetTimeStamp();
                    digest = this.GenerateDigest(
                        username, apiKey, secretKey, timestamp
                    );
                    request = this.BuildRequest(
                        username, timestamp, digest, email, type
                    );
                    response = this.getEmailVerifyRecord(url + request);
                }
                this.PrintResponse(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Error occurred\r\nCannot Email Verify Record for "
                    + email
                );
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        protected string GenerateDigest(
            string username, string apiKey, string secretKey, long timestamp
        )
        {
            string data = username + timestamp + apiKey;
            HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secretKey));
            string hex = BitConverter.ToString(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(data))
            );
            return hex.Replace("-", "").ToLower();
        }

        protected string BuildRequest(
            string username, long timestamp, string digest, string emailAddress, string type
        )
        {
            UserData ud = new UserData();
            ud.u = username;
            ud.t = timestamp;
            string userData = JsonConvert.SerializeObject(ud, Formatting.None);
            var userDataBytes = System.Text.Encoding.UTF8.GetBytes(userData);
            string userDataBase64 = System.Convert.ToBase64String(userDataBytes);

            StringBuilder requestString = new StringBuilder();
            requestString.Append("requestObject=");
            requestString.Append(Uri.EscapeDataString(userDataBase64));
            requestString.Append("&digest=");
            requestString.Append(Uri.EscapeDataString(digest));
            requestString.Append("&emailAddress=");
            requestString.Append(emailAddress);
            requestString.Append("&validateDNS=true");
            requestString.Append("&validateSMTP=true");
            requestString.Append("&checkCatchAll=true");
            requestString.Append("&checkFree=true");
            requestString.Append("&checkDisposable=true");
            requestString.Append("&type=");
            requestString.Append(Uri.EscapeDataString(type));
            requestString.Append("&outputFormat=json");

            return requestString.ToString();
        }

        protected string getEmailVerifyRecord(string url)
        {
            string response;
            try
            {
                WebRequest wr = WebRequest.Create(url);
                WebResponse wp = wr.GetResponse();

                using (Stream data = wp.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        response = reader.ReadToEnd();
                    }
                }
                wp.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            return response;
        }

        protected void PrintResponse(string response)
        {
            dynamic responseObject = JsonConvert.DeserializeObject(response);

            if (responseObject.EmailVerifyRecord != null)
            {
                var emailVR = responseObject.EmailVerifyRecord;
                Console.Write(emailVR);
                Console.WriteLine("--------------------------------");
                return;
            }

            Console.WriteLine(response);
        }

        protected long GetTimeStamp()
        {
            return (long)(DateTime.UtcNow.Subtract(
                new DateTime(1970, 1, 1)
            ).TotalMilliseconds);
        }
    }

    class UserData
    {
        public string u { get; set; }
        public long t { get; set; }
    }
}