#######################
# Use a JSON resource #
#######################
$uri = "https://www.whoisxmlapi.com/reverse-whoi-api/"`
        +"search.php?mode=preview"`
        +"&term1=wikimedia"`
        +"&username=your_whois_api_username&password=your_whois_api_password"`
        +"&outputFormat=json"


$j = Invoke-WebRequest -Uri $uri
echo "JSON:`n---" $j.content "`n"
