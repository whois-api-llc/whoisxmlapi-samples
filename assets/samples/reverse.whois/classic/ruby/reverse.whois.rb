require 'open-uri'
require 'json'

username = 'your whois api username'
password = 'your whois api password'
term =  'wikimedia'
mode = 'preview'
format = 'json'
url = 'https://whoisxmlapi.com/reverse-whois-api/search.php?'
url += 'mode=' + mode + '&username=' + username
url += '&password=' + password + '&outputFormat=' + format
url += '&term1=' + term

def print_response(response)
  response_hash = JSON.parse(response)
  puts JSON.pretty_generate(response_hash)
end

response = open(url).read
print_response(response)