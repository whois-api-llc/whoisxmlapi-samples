$username = "your whois api username"
$password = "your whois api password"

$uri = "https://www.whoisxmlapi.com/reverse-whois-api/search.php?"`
        +"term1=sample"`
        +"&mode=sample_purchase"`
        +"&username=" + $username`
        +"&password=" + $password

$j = Invoke-WebRequest -Uri $uri 

echo $j | ConvertFrom-Json | convertto-json