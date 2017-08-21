require 'json'
require 'yaml'		# only needed to print the returned result in a very pretty way
require 'uri'
require 'openssl'
require "net/https"

########################
# Fill in your details #
########################
username = "YOUR_USERNAME"
password = "YOUR_PASSWORD"

#######################
# Use a JSON resource #
#######################
format = "JSON"
url = 'https://www.whoisxmlapi.com/reverse-whois-api/search.php'

content = {
    "terms" => [
        {
        section: "Registrant",
        attribute: "Name",
        matchType: "BeginsWith",
        value: "Mark",
        exclude:"true"
        },
        {
        section: "Technical",
        attribute: "Country",
        matchType: "Anywhere",
        value: "US",
        exclude:"true"
        }
    ],
    recordsCounter: "false",
    username: username,
    password: password ,
    output_format: format,
}

uri = URI.parse(url)
http = Net::HTTP.new(uri.host, uri.port)

# connect using ssl
http.use_ssl = true
http.verify_mode = OpenSSL::SSL::VERIFY_NONE
request = Net::HTTP::Post.new(uri.request_uri)

# set headers
request.add_field('Content-Type', 'application/json')
request.add_field("Accept", "application/json")
request.body = content.to_json

# get response
response = http.request(request)

# print pretty parsed json
puts JSON.parse(response.body).to_yaml
