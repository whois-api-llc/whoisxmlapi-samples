import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;

import javax.net.ssl.HttpsURLConnection;

import com.google.gson.*;

public class BrandAlertV2Sample
{
    private String apiKey;

    public static void main(String[] args) throws Exception
    {
        BrandAlertV2Sample query = new BrandAlertV2Sample();

        // Fill in your details
        query.setApiKey("Your brand alert api key");

        try {
            String responseStringPost = query.sendPost();
            query.prettyPrintJson(responseStringPost);
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String getApiKey(boolean quotes)
    {
        if (quotes)
            return "\"" + this.getApiKey() + "\"";
        else
            return this.getApiKey();
    }

    // HTTP POST request
    public String sendPost() throws Exception
    {
        String userAgent = "Mozilla/5.0";
        String url = "https://brand-alert.whoisxmlapi.com/api/v2";

        URL obj = new URL(url);
        HttpsURLConnection con = (HttpsURLConnection) obj.openConnection();

        con.setRequestMethod("POST");
        con.setRequestProperty("User-Agent", userAgent);

        // Send POST request
        con.setDoOutput(true);
        DataOutputStream wr = new DataOutputStream(con.getOutputStream());
        wr.writeBytes(this.getTerms());
        wr.flush();
        wr.close();

        System.out.println("\nSending 'POST' request to URL : " + url);

        BufferedReader in = new BufferedReader(
                new InputStreamReader(con.getInputStream()));

        String inputLine;
        StringBuilder response = new StringBuilder();

        while ((inputLine = in.readLine()) != null) {
            response.append(inputLine);
        }
        in.close();

        return response.toString();
    }

    public void setApiKey(String apiKey)
    {
        this.apiKey = apiKey;
    }

    private String getTerms()
    {
        String excludeTerms = "\"excludeSearchTerms\": [\".com\", \".us\"]";
        String includeTerms = "\"includeSearchTerms\": [\"test\", \".info\"]";
        String options = this.getRequestOptions();

        return
            "{ " + includeTerms + ", " + excludeTerms + ", "  + options +" }";
    }

    private String getApiKey()
    {
        return this.apiKey;
    }

    private String getRequestOptions()
    {
        return "\"sinceDate\": \"2018-07-12\","
                + "\"mode\": \"purchase\","
                + "\"responseFormat\": \"json\","
                + "\"apiKey\": " + this.getApiKey(true);
    }

    private void prettyPrintJson(String jsonString)
    {
        Gson gson = new GsonBuilder().setPrettyPrinting().create();

        JsonParser jp = new JsonParser();
        JsonElement je = jp.parse(jsonString);
        String prettyJsonString = gson.toJson(je);

        System.out.println("\n\n" + prettyJsonString);
        System.out.println("----------------------------------------");
    }
}
