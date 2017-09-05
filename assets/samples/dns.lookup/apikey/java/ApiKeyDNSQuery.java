import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import org.apache.commons.codec.binary.Base64;
import org.apache.commons.codec.binary.Hex;

public class DnsLookupApiKey {
    public static final String DOMAIN = "test.com";
    public static final String KEY = "Your whois api public key";
    public static final String SECRET = "Your whois api secret key";
    public static final String USERNAME = "Your whois api username";
    public static final String TYPE = "_all";

    public static void main(String[] args) throws Exception {
        long time = System.currentTimeMillis();

        String json = "{\"t\":" + time + ",\"u\":\"" + USERNAME + "\"}";
        String req=new String(new Base64(true).encodeBase64(json.getBytes()));

        String algo = "HmacMD5";
        String data = USERNAME + time + KEY;
        SecretKeySpec spec = new SecretKeySpec(SECRET.getBytes("UTF-8"),algo);
        Mac mac = Mac.getInstance(spec.getAlgorithm());
        mac.init(spec);
        String hmac =
                new String(Hex.encodeHex(mac.doFinal(data.getBytes("UTF-8"))));
        String url = "http://www.whoisxmlapi.com/whoisserver/DNSService"
                + "?requestObject=" + req + "&digest=" + hmac
                + "&domainName=" + DOMAIN + "&type=" + TYPE;
        try (java.util.Scanner s =
                     new java.util.Scanner(new java.net.URL(url).openStream())) {
            System.out.println(s.useDelimiter("\\A").next());
        }
    }
}