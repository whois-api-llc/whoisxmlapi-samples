$domain = "example.com"
$password = "your whois api password"
$username = "your whois api username"

$uri = "https://www.whoisxmlapi.com/whoisserver/WhoisService?"`
     + "domainName=$($domain)&username=$($username)&password=$($password)"`
     + "&cmd=GET_DN_AVAILABILITY"

echo (Invoke-WebRequest -Uri $uri).Content