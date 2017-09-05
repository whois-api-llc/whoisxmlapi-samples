public class EmailAPIQuery {
        public static void main(String[]args){
            String API_URL = 
                "http://www.whoisxmlapi.com/whoisserver/EmailVerifyService";
            String dns = "true", smtp = "true", catchAll = "true";
            String free ="true", disposable = "true";
            String email = "support@whoisxmlapi.com";
            String username="your whois api username";
            String password="your whois api password";
            String url = API_URL + "?emailAddress=" + email 
                + "&validateDNS=" + dns + "&outputFormat=json"
                + "&validateSMTP=" + smtp + "&checkCatchAll=" + catchAll
                + "&checkFree=" + free + "&userName=" + username 
                + "&password=" + password + "&checkDisposable=" + disposable;
            try (java.util.Scanner s = 
                new java.util.Scanner(new java.net.URL(url).openStream())) {
                System.out.println(s.useDelimiter("\\A").next());

            } catch (Exception ex) {
                ex.printStackTrace();
            }
        }
}