require 'json'
require 'net/https'
require 'openssl'
require 'uri'
require 'yaml' # only needed to print the returned result in a very pretty way

url = 'https://reverse-whois-api.whoisxmlapi.com/api/v2'

########################
# Fill in your details #
########################
key = 'Your reverse whois api key'

params_advanced = {
  advancedSearchTerms: [
    {
      field: 'RegistrantContact.Name',
      term: 'Test'
    }
  ],
  mode: 'purchase',
  sinceDate: '2018-07-15'
}

params_basic = {
  basicSearchTerms: {
    include: %w[
      test
    ],
    exclude: %w[
      whois
      api
    ]
  },
  mode: 'purchase',
  sinceDate: '2018-07-15'
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
request.add_field('X-Authentication-Token', key)

# Basic search

request.body = params_basic.to_json
response = http.request(request)

# Print pretty parsed json
puts 'Basic:'
puts JSON.parse(response.body).to_yaml

# Advanced search

request.body = params_advanced.to_json
response = http.request(request)

puts "\nAdvanced:"
puts JSON.parse(response.body).to_yaml