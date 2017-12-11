########################
# Fill in your details #
########################
$apiKey = "your_email_verification_api_key"

$emailAddress = "support@whoisxmlapi.com"

$uri = "https://emailverification.whoisxmlapi.com/api/v1?"`
    + "emailAddress=$emailAddress"`
    + "&apiKey=$apiKey"


#######################
# Use a JSON resource #
#######################

$j = Invoke-WebRequest -Uri $uri
echo "JSON:`n---" $j.content "`n"