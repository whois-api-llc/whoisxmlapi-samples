$url = 'https://www.whoisxmlapi.com/whoisserver/WhoisService'

########################
# Fill in your details #
########################

$apiKey = 'Your whois api key'
$domain = 'whoisxmlapi.com'

$uri = $url `
     + '?domainName=' + [uri]::EscapeDataString($domain) `
     + '&username=' + [uri]::EscapeDataString($username) `
     + '&password=' + [uri]::EscapeDataString($password) `
     + '&outputFormat=json'

$uri = $url `
     + '?domainName=' + [uri]::EscapeDataString($domain) `
     + '&apiKey=' + [uri]::EscapeDataString($apiKey) `
     + '&outputFormat=json';

$res = Invoke-WebRequest -Uri $uri -UseBasicParsing | ConvertFrom-Json

if ([bool]($res.PSObject.Properties.name -match 'WhoisRecord')) {
    echo "Domain Name: $($res.WhoisRecord.domainName)"
    echo "Contact Email: $($res.WhoisRecord.contactEmail)"
} else {
    echo $res
}