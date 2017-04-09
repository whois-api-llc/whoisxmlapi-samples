$domain = "example.com"
$password = "your whois api password"
$username = "your whois api username"

$uri = "https://www.whoisxmlapi.com/whoisserver/WhoisService?"`
     + "domainName=$($domain)&username=$($username)&password=$($password)"

echo (Invoke-WebRequest -Uri $uri).Content