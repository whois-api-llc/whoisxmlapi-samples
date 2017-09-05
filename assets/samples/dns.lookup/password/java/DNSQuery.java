public class DNSQuery {
        public static void main(String[]args){
            String API_URL = 
                "http://www.whoisxmlapi.com/whoisserver/DNSService";
            String type = "_all";
            String domainName = "test.com";
            String username="Your whois api username";
            String password="Your whois api password";
            String url = API_URL + "?type=" + type 
                + "&domainName=" + domainName + "&outputFormat=json"
                + "&userName=" + username + "&password=" + password;
            try (java.util.Scanner s = 
                new java.util.Scanner(new java.net.URL(url).openStream())) {
                System.out.println(s.useDelimiter("\\A").next());

            } catch (Exception ex) {
                ex.printStackTrace();
            }
        }
}