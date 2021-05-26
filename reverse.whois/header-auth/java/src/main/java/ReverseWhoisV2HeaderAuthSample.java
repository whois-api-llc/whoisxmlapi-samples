import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;

import javax.net.ssl.HttpsURLConnection;

import com.google.gson.*;

public class ReverseWhoisV2HeaderAuthSample
{
    private String apiKey;

    public static void main(String[] args) throws Exception
    {
        ReverseWhoisV2HeaderAuthSample query =
            new ReverseWhoisV2HeaderAuthSample();

        // Fill in your details
        query.setApiKey("Your reverse whois api key");

        try {
            String responseStringPost = query.sendPost();
            query.prettyPrintJson(responseStringPost);

            responseStringPost = query.sendPost(true);
            query.prettyPrintJson(responseStringPost);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String getApiKey()
    {
        return this.apiKey;
    }

    public void setApiKey(String apiKey)
    {
        this.apiKey = apiKey;
    }

    public String sendPost() throws Exception
    {
        return sendPost(false);
    }

    // HTTP POST request
    public String sendPost(boolean isAdvanced) throws Exception
    {
        String userAgent = "Mozilla/5.0";
        String url = "https://reverse-whois-api.whoisxmlapi.com/api/v2";

        URL obj = new URL(url);
        HttpsURLConnection con = (HttpsURLConnection) obj.openConnection();

        con.setRequestMethod("POST");
        con.setRequestProperty("User-Agent", userAgent);
        con.setRequestProperty("X-Authentication-Token", this.getApiKey());

        String terms = isAdvanced ? getAdvancedTerms() : getBasicTerms();

        // Send POST request
        con.setDoOutput(true);
        DataOutputStream wr = new DataOutputStream(con.getOutputStream());
        wr.writeBytes(terms);
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

    private String getAdvancedTerms()
    {
        String field = "\"field\": \"RegistrantContact.Name\"";
        String term = "\"term\": \"Test\"";
        String options = this.getRequestOptions();
        String searchTerms = "{ " + field + ", " + term + " }";

        String terms =
                "\"advancedSearchTerms\": [" + searchTerms + "]";

        return "{ " + terms + ", "  + options +" }";
    }

    private String getBasicTerms()
    {
        String exclude = "\"exclude\": [\"pay\"]";
        String include = "\"include\": [\"test\"]";
        String options = this.getRequestOptions();

        String terms =
                "\"basicSearchTerms\": {" + include + ", " + exclude + "}";

        return "{ " + terms + ", "  + options +" }";
    }

    private String getRequestOptions()
    {
        return "\"sinceDate\": \"2018-07-16\","
                + "\"mode\": \"purchase\"";
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