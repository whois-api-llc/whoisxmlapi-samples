public class BrandAlertApiQuery {
    public static void main(String[]args){
        String API_URL="https://www.whoisxmlapi.com/brand-alert-api/search.php";
        String username="Your whois api username";
        String password="Your whois api password";
        String term1 = "cinema";
        String rows = "100";
        String url = API_URL+
                "?username=" + username +
                "&password=" + password +
                "&term1=" + term1 +
                "&rows=" + rows +
                "&output_format=json";
        try (java.util.Scanner s = 
                 new java.util.Scanner(new java.net.URL(url).openStream())){
            System.out.println(s.useDelimiter("\\A").next());
        } catch (Exception ex) {
            ex.printStackTrace();
        } 
    }
}