$url = 'https://www.whoisxmlapi.com/whoisserver/DNSService'

$apiKey = 'Your dns lookup api key'
$domainName = 'google.com'
$type = '_all'

$uri = $url`
     + '?type=' + [uri]::EscapeDataString($type)`
     + '&domainName=' + [uri]::EscapeDataString($domainName)`
     + '&apiKey=' + [uri]::EscapeDataString($apiKey)

#######################
# Use an XML resource #
#######################

$j = Invoke-WebRequest -Uri $uri
echo $j.content

#######################
# Use a JSON resource #
#######################

$uri = $uri + '&outputFormat=json'

$j = Invoke-WebRequest -Uri $uri
echo $j.content | convertfrom-json | convertto-json -depth 10
