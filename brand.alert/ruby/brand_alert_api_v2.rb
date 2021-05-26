require 'json'
require 'net/https'
require 'openssl'
require 'uri'
require 'yaml' # only needed to print the returned result in a very pretty way

url = 'https://brand-alert.whoisxmlapi.com/api/v2'

########################
# Fill in your details #
########################
key = 'Your brand alert api key'

params = {
  includeSearchTerms: [
    'whois'
  ],
  excludeSearchTerms: [
    '.win',
    '.tz',
    ".com"
  ],
  mode: 'purchase',
  apiKey: key,
  responseFormat: 'json',
  sinceDate: '2018-07-10'
}

uri = URI.parse(url)
http = Net::HTTP.new(uri.host, uri.port)

# Connect using ssl
http.use_ssl = true
http.verify_mode = OpenSSL::SSL::VERIFY_NONE
request = Net::HTTP::Post.new(uri.request_uri)

# Set headers
request.add_field('Content-Type', 'application/json')
request.add_field('Accept', 'application/json')
request.body = params.to_json

# Get response
response = http.request(request)

# Print pretty parsed json
puts JSON.parse(response.body).to_yaml
