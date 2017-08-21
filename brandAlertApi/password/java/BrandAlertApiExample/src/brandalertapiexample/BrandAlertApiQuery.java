import java.io.IOException;
import java.io.StringReader;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
import org.w3c.dom.Document;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

public class BrandAlertApiQuery {
    public static void main(String[]args){
        String API_URL="https://www.whoisxmlapi.com/brand-alert-api/search.php";

        String username=null, password=null;

        String term1 = "cinema";
        String term2 = "club";
        String rows = "100";

        String url = API_URL+
                "?username=" + username +
                "&password=" + password +
                "&term1=" + term1 +
                "&term2=" + term2 +
                "&rows=" + rows +
                "&output_format=xml";

        HttpClient httpclient =null;
        try {
            httpclient = new DefaultHttpClient();
            HttpGet httpget = new HttpGet(url);
            System.out.println("executing request " + httpget.getURI());

            // Create a response handler
            ResponseHandler<String> responseHandler = new BasicResponseHandler();
            String responseBody = httpclient.execute(httpget, responseHandler);
            System.out.println(responseBody);
            System.out.println("----------------------------------------");

            //parse

            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            InputSource is = new InputSource();
            is.setCharacterStream(new StringReader(responseBody));
            Document doc = db.parse(is);

            System.out.println("Root element " + doc.getDocumentElement().getNodeName());

        } catch (SAXException ex) {
            ex.printStackTrace();
        } catch (ParserConfigurationException ex) {
            ex.printStackTrace();;
        } catch (IOException ex) {
           ex.printStackTrace();
        } finally{
            if(httpclient!=null)httpclient.getConnectionManager().shutdown();
        }
    }
}
