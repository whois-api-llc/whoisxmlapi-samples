public class WhoisApiQuery {
    public static final String DOMAIN = "example.com";
    public static final String PASSWORD = "your whois api password";
    public static final String USERNAME = "your whois api username";

    public static void main(String[] args) throws Exception {
        String url = "http://www.whoisxmlapi.com/whoisserver/WhoisService"
                   + "?domainName=" + DOMAIN + "&userName=" + USERNAME
                   + "&password=" + PASSWORD;
        try (java.util.Scanner s =
                new java.util.Scanner(new java.net.URL(url).openStream())) {
            System.out.println(s.useDelimiter("\\A").next());
        }
    }
}
