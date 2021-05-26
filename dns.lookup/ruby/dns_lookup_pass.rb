require 'erb'
require 'json'
require 'net/https'
require 'uri'

########################
# Fill in your details #
########################
apiKey = 'Your dns lookup api key'

domains = %w[
  google.com
  whoisxmlapi.com
  twitter.com
]

url = 'https://whoisxmlapi.com/whoisserver/DNSService'
type = 'TXT'

def build_request(apiKey, type, domain)
  'type=' + ERB::Util.url_encode(type) +
    '&apiKey=' + ERB::Util.url_encode(apiKey) +
    '&outputFormat=json' +
    '&domainName=' + ERB::Util.url_encode(domain)
end

def print_response(response)
  response_hash = JSON.parse(response)
  puts JSON.pretty_generate(response_hash)
end

domains.each do |domain|
  request_string = build_request(apiKey, type, domain)
  response = Net::HTTP.get(URI.parse(url + '?' + request_string))
  print_response(response)
  puts "--------------------------------\n"
end
