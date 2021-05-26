$url = 'https://domain-availability.whoisxmlapi.com/api/v1'

$apiKey = 'Your domain availability api key'
$domain = 'whoisxmlapi.com'
$format = 'json'

$uri = $url`
     + '?domainName=' + [uri]::EscapeDataString($domain)`
     + '&apiKey=' + [uri]::EscapeDataString($apiKey)`
     + '&outputFormat=' + [uri]::EscapeDataString($format)

$j = Invoke-WebRequest -Uri $uri -UseBasicParsing | `
     ConvertFrom-Json

if ([bool]($j.PSObject.Properties.name -match 'DomainInfo')) {
    echo "Domain name: $($j.DomainInfo.domainName)"
    echo "Domain availability: $($j.DomainInfo.domainAvailability)"
} else {
    echo $j
}
