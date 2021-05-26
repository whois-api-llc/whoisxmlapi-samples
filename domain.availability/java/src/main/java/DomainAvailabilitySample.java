import java.io.IOException;
import java.io.StringReader;
import java.net.URLEncoder;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.ResponseHandler;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;

import org.w3c.dom.Document;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

/**
 *
 * @author jonz
 */
public class DomainAvailabilitySample
{
    public static void main(String[]args) throws IOException
    {
        String API_URL="https://domain-availability.whoisxmlapi.com/api/v1";

        String domainName = "test.com";
        String mode = "DNS_ONLY"; //or DNS_AND_WHOIS
        String apiKey = "Your domain availability api key";

        CloseableHttpClient httpclient = null;
        try {
            String url =
                API_URL
                + "?domainName=" + URLEncoder.encode(domainName, "UTF-8")
                + "&mode=" + URLEncoder.encode(mode, "UTF-8")
                + "&apiKey=" + URLEncoder.encode(apiKey, "UTF-8");

            httpclient = HttpClients.createDefault();
            HttpGet httpget = new HttpGet(url);
            System.out.println("executing request " + httpget.getURI());

            // Create a response handler
            ResponseHandler<String> responseHandler =
                new BasicResponseHandler();

            String responseBody = httpclient.execute(httpget,responseHandler);
            System.out.println(responseBody);
            System.out.println("----------------------------------------");

            // Parse
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            InputSource is = new InputSource();

            is.setCharacterStream(new StringReader(responseBody));
            Document doc = db.parse(is);

            System.out.println("Root element "
                               + doc.getDocumentElement().getNodeName());

        } catch (SAXException ex) {
            ex.printStackTrace();
        } catch (ParserConfigurationException ex) {
            ex.printStackTrace();
        } catch (IOException ex) {
            ex.printStackTrace();
        } finally{
            if (httpclient != null)
                httpclient.close();
        }
    }
}
