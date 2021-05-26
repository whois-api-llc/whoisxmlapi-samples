$url = 'https://emailverification.whoisxmlapi.com/api/v1'

########################
# Fill in your details #
########################
$apiKey = 'Your email verification api key'

$emailAddress = 'support@whoisxmlapi.com'

$uri = $url`
     + '?emailAddress=' + [uri]::EscapeDataString($emailAddress)`
     + '&apiKey=' + [uri]::EscapeDataString($apiKey)

#########################
# Use the JSON resource #
#########################

$response = Invoke-WebRequest -Uri $uri
echo $response.content | convertfrom-json