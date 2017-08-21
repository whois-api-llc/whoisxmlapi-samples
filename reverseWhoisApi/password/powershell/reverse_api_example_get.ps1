########################
# Fill in your details #
########################

$username = "xxxx"
$password = "xxxx"

#######################
# Use a JSON resource #
#######################

##################
$uri = "https://www.whoisxmlapi.com/reverse-whois-api/search.php?"`
        +"term1=sample"`
        +"&mode=sample_purchase"`
        +"&username=" + $username`
        +"&password=" + $password

$j = Invoke-WebRequest -Uri $uri 

echo $j | ConvertFrom-Json | convertto-json
