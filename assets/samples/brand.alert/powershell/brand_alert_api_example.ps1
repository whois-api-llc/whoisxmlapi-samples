$uri = "https://www.whoisxmlapi.com/brand-alert-api/search.php?"`
+ "term1=cinema"`
+ "&username=Your_whois_api_username"`
+ "&password=Your_whois_api_password"`
+ "&rows=10"`
+ "&page=2"

#######################
# Use a JSON resource #
#######################

$j = Invoke-WebRequest -Uri $uri
echo "JSON:`n---" $j.content "`n"

#######################
# Use a XML resource #
#######################

$uri = $uri + "&output_format=xml"

$j = Invoke-WebRequest -Uri $uri
echo "XML:`n---" $j.content