$url = 'https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices'

########################
# Fill in your details #
########################

$apiKey = 'Your bulk whois api key'

$domains = @('google.com', 'whoisxmlapi.com')

function Bulk-Whois-Json-Post ($Path, $Body) {
    return Invoke-WebRequest -Uri ($Url + $Path) -Method POST -Body $Body `
                -ContentType 'application/json'
}

function Create-Bulk-Whois-Request ([string[]] $Domains) {
    $body = @{
        apiKey = $apiKey
        outputFormat = 'json'
        domains = $Domains
    } | ConvertTo-Json

    $result = Bulk-Whois-Json-Post -Path '/bulkWhois' -Body $body `
                | ConvertFrom-Json

    if ($result.messageCode -ne 200) {
        throw "$($result.messageCode): $($result.message)"
    }

    return $result.requestId
}

function Get-Bulk-Whois-Records ($Id, $Start, $Max) {
    $body = @{
        apiKey = $apiKey
        outputFormat = 'json'
        requestId = $Id
        startIndex = $Start
        maxRecords = $Max
    } | ConvertTo-Json

    return Bulk-Whois-Json-Post -Path '/getRecords' -Body $body `
                | ConvertFrom-Json
}

########################
# Get the data         #
########################

echo 'Requesting bulk processing...'

$id = Create-Bulk-Whois-Request $domains

echo "    request created: $($id)"

echo 'Waiting for processing to finish...'

Do {
    $recordsLeft = (Get-Bulk-Whois-Records $id $domains.Count+1 0).recordsLeft
    echo "    records left: $($recordsLeft)"
    if ($recordsLeft -eq 0) {
        break
    }
    Start-Sleep -s 5
} While (1)

echo 'Downloading data...'

$records = (Get-Bulk-Whois-Records $id 1 $domains.Count).whoisRecords

echo $records | ConvertTo-Json -depth 10
