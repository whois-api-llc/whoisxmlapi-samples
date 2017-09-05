$username = "your_whois_api_username"
$key = "your_whois_api_key"
$secret = "your_whois_api_secret_key"

$emailAddress = "support@whoisxmlapi.com"

$time = [DateTimeOffset]::Now.ToUnixTimeMilliseconds()
$req=[Text.Encoding]::UTF8.GetBytes("{`"t`":$($time),`"u`":`"$($username)`"}")
$req = [Convert]::ToBase64String($req)

$data = $username + $time + $key
$hmac = New-Object System.Security.Cryptography.HMACMD5
$hmac.key = [Text.Encoding]::UTF8.GetBytes($secret)
$hash = $hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes($data))
$digest = [BitConverter]::ToString($hash).Replace('-','').ToLower()

$uri = "https://www.whoisxmlapi.com/whoisserver/EmailVerifyService?"`
    + "&requestObject=$($req)&digest=$($digest)"`
    + "&emailAddress=$($emailAddress)"`
    + "&validateDNS=1"`
    + "&validateSMTP=1"`
    + "&checkCatchAll=1"`
    + "&checkFree=1"`
    + "&checkDisposable=1"`

$uri = $uri + "&outputFormat=json"
echo "JSON:`n---" (Invoke-WebRequest -Uri $uri).Content