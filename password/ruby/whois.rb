require 'open-uri'
require 'json'
require 'rexml/document'
require 'rexml/xpath'
require 'yaml'		# only needed to print the returned result in a very pretty way

########################
# Fill in your details #
########################
username = "YOUR_USERNAME"
password = "YOUR_PASSWORD"
domain = "google.com"

#######################
# Use a JSON resource #
#######################
format = "JSON"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format

# Open the resource
buffer = open(url).read

# Parse the JSON result
result = JSON.parse(buffer)

# Print out a nice informative string
puts "XML:\n" + result.to_yaml + "\n"

#######################
# Use an XML resource #
#######################
format = "XML"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format

# Open the resource
buffer = open(url).read

# Parse the XML result
result = REXML::Document.new(buffer)

# Get a few data members and make sure they aren't nil
if ((errorMessage = REXML::XPath.first(result, "/ErrorMessage/msg")) != nil)
	puts "JSON:\nErrorMessage:\n\t" + errorMessage.text
else
	registrantName = (registrantName = REXML::XPath.first(result, "/WhoisRecord/registrant/name")) == nil ? '' : registrantName.text
	domainName = (domainName = REXML::XPath.first(result, "/WhoisRecord/domainName")) == nil ? '' : domainName.text
	createdDate = (createdDate = REXML::XPath.first(result, "WhoisRecord/createdDate")) == nil ? '' : createdDate.text
	
	# Print out a nice informative string
	puts "JSON:\n'" + registrantName + "' created " + domainName + " on " + createdDate
end

