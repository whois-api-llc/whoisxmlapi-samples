$uri = "https://www.whoisxmlapi.com/whoisserver/"`
       +"WhoisService?domainName=google.com"`
       +"&username=xxxx&password=xxxxxxx"`
       +"&outputFormat=json"

$j = Invoke-WebRequest -Uri $uri -UseBasicParsing  | `
 ConvertFrom-Json

if ([bool]($j.PSObject.Properties.name -match "WhoisRecord")) {
    echo "Domain Name: $($j.WhoisRecord.domainName)"
    echo "Contact Email: $($j.WhoisRecord.contactEmail)"
} else {
    echo $j
}