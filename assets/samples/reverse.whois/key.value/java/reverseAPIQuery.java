import javax.net.ssl.HttpsURLConnection;
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.URL;

public class ReverseApiQuery {
    public static void main(String[] args) throws Exception {
        URL urlObj = new URL(
                "https://www.whoisxmlapi.com/reverse-whois-api/search.php"
        );
        HttpsURLConnection con = (HttpsURLConnection)urlObj.openConnection();
        con.setRequestMethod("POST"); con.setDoOutput(true);
        DataOutputStream wr = new DataOutputStream(con.getOutputStream());
        wr.writeBytes("{\"terms\": [{\"section\": \"Registrant\","
                + "\"attribute\": \"Email\", \"value\": "
                + "\"support@whoisxmlapi.com\", \"matchType\": \"exact\", "
                + "\"exclude\": \"false\"}], \"recordsCounter\": \"false\","
                + " \"outputFormat\": \"json\", \"username\":"
                + " \"your whois api username\", \"password\":"
                + " \"your whois api password\", \"rows\": \"10\", "
                + "\"searchType\": \"current\"}");
        wr.flush(); wr.close();
        BufferedReader reader = new BufferedReader(
                new InputStreamReader(con.getInputStream())
        );
        String chunk; StringBuffer response = new StringBuffer();
        while((chunk = reader.readLine()) != null) {response.append(chunk);}
        reader.close();
        System.out.println(response);
    }
}