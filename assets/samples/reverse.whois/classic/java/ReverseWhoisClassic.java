public class ReverseWhoisClassic {
        public static void main(String[]args){
            String API_URL = 
                "https://www.whoisxmlapi.com/reverse-whois-api/search.php";
            String term = "wikimedia";
            String username="your whois api username";
            String password="your whois api password";
            String url = API_URL + "?mode=preview"
                + "&term1=" + term + "&outputFormat=json"
                + "&username=" + username + "&password=" + password;
            try (java.util.Scanner s = 
                new java.util.Scanner(new java.net.URL(url).openStream())) {
                System.out.println(s.useDelimiter("\\A").next());

            } catch (Exception ex) {
                ex.printStackTrace();
            }
        }
}