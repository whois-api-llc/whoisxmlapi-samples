require 'erb'
require 'json'
require 'rexml/document'
require 'rexml/xpath'
require 'net/https'
require 'yaml'

########################
# Fill in your details #
########################
api_key = 'Your Whois API key'
domain = 'google.com'

api_url = 'https://www.whoisxmlapi.com/whoisserver/WhoisService'

url = api_url + 
      '?domainName=' + ERB::Util.url_encode(domain) +
      '&apiKey=' + ERB::Util.url_encode(api_key) +
      '&outputFormat='

#########################
# Use the JSON resource #
#########################
format = 'JSON'

# Open the resource
buffer = Net::HTTP.get(URI.parse(url + ERB::Util.url_encode(format)))

# Parse the JSON result
result = JSON.parse(buffer)

# Print out a nice informative string
puts "XML:\n" + result.to_yaml + "\n"

#######################
# Use an XML resource #
#######################
format = 'XML'

# Open the resource
buffer = Net::HTTP.get(URI.parse(url + ERB::Util.url_encode(format)))

# Parse the XML result
result = REXML::Document.new(buffer)

# Get a few data members and make sure they aren't nil
if !(error_msg = REXML::XPath.first(result, '/ErrorMessage/msg')).nil?
  puts "JSON:\nErrorMessage:\n\t" + error_msg.text
else
  el_date = '/WhoisRecord/createdDate'
  el_dom = '/WhoisRecord/domainName'
  el_reg = '/WhoisRecord/registrant/name'

  reg = (reg = REXML::XPath.first(result, el_reg)).nil? ? '' : reg.text
  dom = (dom = REXML::XPath.first(result, el_dom)).nil? ? '' : dom.text
  date = (date = REXML::XPath.first(result, el_date)).nil? ? '' : date.text

  # Print out a nice informative string
  puts "JSON:\n'" + reg + "' created " + dom + ' on ' + date
end