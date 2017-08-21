$uri = "https://www.whoisxmlapi.com/whoisserver/"`
       +"WhoisService?cmd=GET_DN_AVAILABILITY"`
       +"&domainName=google.com"`
       +"&username=xxxx&password=xxxxxx"`
       +"&outputFormat=json"

$j = Invoke-WebRequest -Uri $uri -UseBasicParsing  | `
 ConvertFrom-Json

if ([bool]($j.PSObject.Properties.name -match "DomainInfo")) {
    echo "Domain name: $($j.DomainInfo.domainName)"
    echo "Domain availability: $($j.DomainInfo.domainAvailability)"
} else {
    echo $j
}