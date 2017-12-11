import java.util.logging.Level;
import java.util.logging.Logger;

import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.HttpMethod;
import org.apache.commons.httpclient.methods.GetMethod;

public class EmailVerificationApi {

    private Logger logger = Logger.getLogger(EmailVerificationApi.class.getName());

    public static void main(String[]args) {
        new EmailVerificationApi().getEmailVerifyRecordUsingApiKey();
    }

    private void getEmailVerifyRecordUsingApiKey() {

        String apiKey = "Your_email_verification_api_apiKey";

        String emailAddress = "support@whoisxmlapi.com";
        getEmailInfo(emailAddress, apiKey);
    }

    private String executeURL(String url) {
        HttpClient c = new HttpClient();
        System.out.println(url);
        HttpMethod m = new GetMethod(url);
        String res = null;
        try {
            c.executeMethod(m);
            res = new String(m.getResponseBody());
        } catch (Exception e) {
            logger.log(Level.SEVERE, "Cannot get url", e);
        } finally {
            m.releaseConnection();
        }
        return res;
    }

    public void getEmailInfo(String emailAddress, String apiKey) {
        StringBuilder sb = new StringBuilder();
        sb.append("https://emailverification.whoisxmlapi.com/api/v1?");
        sb.append("apiKey=");
        sb.append(apiKey);
        sb.append("&emailAddress=");
        sb.append(emailAddress);
        String url = sb.toString();

        String result = executeURL(url);
        if (result != null) {
            logger.log(Level.INFO, "result: " + result);
        }
    }

}
