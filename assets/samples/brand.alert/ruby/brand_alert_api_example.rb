require 'open-uri'
require 'json'
require 'yaml'	

username = "Your whois api username"
password = "Your whois api password"
term1 = "cinema"
exclude_term1 = 'movie'
exclude_term2 = 'online'

format = "JSON"
url = 'https://www.whoisxmlapi.com/brand-alert-api/search.php?' +
    'term1=' + term1 +
    '&username=' + username +
    '&password=' + password +
    '&output_format=' + format +
    '&exclude_term1=' + exclude_term1
    '&exclude_term2=' + exclude_term2

# Open the resource
buffer = open(url).read

# Parse the JSON result
result = JSON.parse(buffer)

# Print out a nice informative string
puts "JSON:\n" + result.to_yaml + "\n"
