import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.logging.Level;
import java.util.logging.Logger;

import com.google.gson.*;

import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.HttpMethod;
import org.apache.commons.httpclient.methods.GetMethod;

public class EmailVerificationApiSample
{
    private Logger logger =
            Logger.getLogger(EmailVerificationApiSample.class.getName());

    public static void main(String[]args)
    {
        String apiKey = "Your email verification api key";
        String emailAddress = "support@whoisxmlapi.com";

        EmailVerificationApiSample sample = new EmailVerificationApiSample();

        String result = sample.getEmailInfo(emailAddress, apiKey);

        System.out.println(sample.prettyPrintJson(result));
    }

    public String getEmailInfo(String emailAddress, String apiKey)
    {
        String url = "https://emailverification.whoisxmlapi.com/api/v1";
        String sb;

        try {
            sb = url
               + "?apiKey=" + URLEncoder.encode(apiKey, "UTF-8")
               + "&emailAddress=" + URLEncoder.encode(emailAddress, "UTF-8");
        }
        catch (UnsupportedEncodingException e) {
            logger.log(Level.SEVERE, "exception: " + e);
            return "";
        }

        String result = executeURL(sb);

        if (result != null) {
            logger.log(Level.INFO, "result: " + result);
        }

        return result;
    }

    private String executeURL(String url)
    {
        HttpClient c = new HttpClient();

        logger.log(Level.INFO, "url: " + url);

        HttpMethod m = new GetMethod(url);

        String res = null;
        try {
            c.executeMethod(m);

            BufferedReader reader = new BufferedReader(
                new InputStreamReader(m.getResponseBodyAsStream()));

            StringBuilder stringBuffer = new StringBuilder();

            String str;
            while ((str = reader.readLine()) != null) {
                stringBuffer.append(str);
                stringBuffer.append("\n");
            }

            res = stringBuffer.toString();
        } catch (Exception e) {
            logger.log(Level.SEVERE, "Cannot get url", e);
        } finally {
            m.releaseConnection();
        }

        return res;
    }

    private String prettyPrintJson(String jsonString)
    {
        Gson gson = new GsonBuilder().setPrettyPrinting().create();

        JsonParser jp = new JsonParser();
        JsonElement je = jp.parse(jsonString);

        return gson.toJson(je);
    }
}