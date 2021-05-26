########################
# Fill in your details #
########################

$apiKey = 'Your brand alert api key'

$params = @{
    includeSearchTerms = @(
        'whois',
        'api'
    )
    excludeSearchTerms = @(
        '.ml'
    )
    apiKey = $apiKey
    mode = 'purchase'
    responseFormat = 'json'
    sinceDate = '2018-06-15'
} | ConvertTo-Json

#######################
# POST request        #
#######################

$uri = 'https://brand-alert.whoisxmlapi.com/api/v2'

$response = Invoke-WebRequest -Uri $uri -Method POST -Body $params `
            -ContentType 'application/json'

echo $response.content | convertfrom-json | convertto-json -depth 10
