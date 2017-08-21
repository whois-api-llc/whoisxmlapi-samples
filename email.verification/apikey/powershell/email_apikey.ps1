########################
# Fill in your details #
########################

#$username = "your_whois_api_username"
#$key = "your_whois_api_key"
#$secret = "your_whois_api_secret_key"

$emailAddress = "support@whoisxmlapi.com"

$time = [DateTimeOffset]::Now.ToUnixTimeMilliseconds()
$req=[Text.Encoding]::UTF8.GetBytes("{`"t`":$($time),`"u`":`"$($username)`"}")
$req = [Convert]::ToBase64String($req)

$data = $username + $time + $key
$hmac = New-Object System.Security.Cryptography.HMACMD5
$hmac.key = [Text.Encoding]::UTF8.GetBytes($secret)
$hash = $hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes($data))
$digest = [BitConverter]::ToString($hash).Replace('-','').ToLower()

$validateDNS = "true"
$validateSMTP = "true"
$checkCatchAll = "true"
$checkFree = "true"
$checkDisposable = "true"

$uri = "https://www.whoisxmlapi.com/whoisserver/EmailVerifyService?"`
    + "&requestObject=$($req)&digest=$($digest)"`
    + "&emailAddress=$($emailAddress)"`
    + "&validateDNS=$($validateDNS)"`
    + "&validateSMTP=$($validateSMTP)"`
    + "&checkCatchAll=$($checkCatchAll)"`
    + "&checkFree=$($checkFree)"`
    + "&checkDisposable=$($checkDisposable)"`

#######################
# Use a XML resource #
#######################
#using default outputFormat (xml)

echo "XML:`n---" (Invoke-WebRequest -Uri $uri).Content

#######################
# Use a JSON resource #
#######################

$format = "json"
$uri = $uri + "&outputFormat=json"
echo "XML:`n---" (Invoke-WebRequest -Uri $uri).Content