require 'open-uri'
require 'json'

########################
# Fill in your details #
########################
username = "YOUR_USERNAME"
password = "YOUR_PASSWORD"

domains = [
    'google.com',
    'whoisxmlapi.com',
    'twitter.com'
]

url = 'https://whoisxmlapi.com/whoisserver/DNSService?'
type = "TXT"
format = 'json'

def build_request(username, password, format, type, domain)
  request_string = 'type='
  request_string += type
  request_string += '&username='
  request_string += username
  request_string += '&password='
  request_string += password
  request_string += '&outputFormat='
  request_string += format
  request_string += '&domainName='
  request_string += domain
  return request_string
end

def print_response(response)
  response_hash = JSON.parse(response)
  puts JSON.pretty_generate(response_hash)
end

domains.each do |domain|
  request_string = build_request(username, password, format, type, domain)
  response = open(url + request_string).read
  print_response(response)
  puts "--------------------------------\n"
end

