$username = "your whois api username"
$password = "your whois api password"

$postParams = @{
        terms = @(
            @{
                section ="Admin"
                attribute = "name"
                value = "Brett Branch"
                exclude = "false"
            }
        )
        recordsCounter = "false"
        username = $username
        password = $password
        output_format = "json"
    } | ConvertTo-Json

$uri = 'https://www.whoisxmlapi.com/reverse-whois-api/search.php'

$response = Invoke-WebRequest -Uri $uri -Method POST -Body $postParams -ContentType "application/json"

echo $response.content | convertfrom-json | convertto-json -depth 10