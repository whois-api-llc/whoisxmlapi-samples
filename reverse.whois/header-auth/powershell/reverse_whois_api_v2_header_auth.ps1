########################
# Fill in your details #
########################

$apiKey = 'Your reverse whois api key'

$paramsAdvanced = @{
    advancedSearchTerms = @(
        @{
            field = 'RegistrantContact.Name'
            term = 'Test'
        }
    )
    mode = 'purchase'
    sinceDate = '2018-07-15'
} | ConvertTo-Json

$paramsBasic = @{
    basicSearchTerms = @{
        include = @(
            'test'
        )
        exclude = @(
            'whois',
            'api'
        )
    }
    mode = 'purchase'
    sinceDate = '2018-07-15'
} | ConvertTo-Json

#######################
# POST request        #
#######################

$uri = 'https://reverse-whois-api.whoisxmlapi.com/api/v2'

$response = Invoke-WebRequest -Uri $uri -Method POST -Body $paramsBasic `
            -ContentType 'application/json' `
            -Headers @{'X-Authentication-Token'=$apiKey}

echo 'Basic:'
echo $response.content | convertfrom-json | convertto-json -depth 10

$response = Invoke-WebRequest -Uri $uri -Method POST -Body $paramsAdvanced `
            -ContentType 'application/json' `
            -Headers @{'X-Authentication-Token'=$apiKey}

echo 'Advanced:'
echo $response.content | convertfrom-json | convertto-json -depth 10