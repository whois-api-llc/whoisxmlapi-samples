require 'open-uri'
require 'json'

username = 'your whois api username'
password = 'your whois api password'
domain =  'twitter.com'
type = '_all'
format = 'json'
url = 'https://whoisxmlapi.com/whoisserver/DNSService?'
url += 'type=' + type + '&username=' + username
url += '&password=' + password + '&outputFormat=' + format
url += '&domainName=' + domain

def print_response(response)
  response_hash = JSON.parse(response)
  puts JSON.pretty_generate(response_hash)
end

response = open(url).read
print_response(response)