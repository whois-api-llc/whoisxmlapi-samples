$username = "your whois api username"
$password = "your whois api password"

$emailAddress = "support@whoisxmlapi.com"

$uri = "https://www.whoisxmlapi.com/whoisserver/EmailVerifyService?"`
    + "emailAddress=$emailAddress"`
    + "&validateDNS=true"`
    + "&validateSMTP=true"`
    + "&checkCatchAll=true"`
    + "&checkFree=true"`
    + "&checkDisposable=true"`
    + "&username=$username"`
    + "&password=$password"`
    + "&outputFormat=json"

$j = Invoke-WebRequest -Uri $uri
echo "JSON:`n---" $j.content "`n"