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

emailAddress = "support@whoisxmlapi.com"
checkFree = 'true'
validateDNS = 'true'
validateSMTP = 'true'
checkCatchAll = 'true'
checkDisposable = 'true'

#######################
# Use a JSON resource #
#######################
url = 'https://www.whoisxmlapi.com/whoisserver/EmailVerifyService?' +
  #required params
    'username=' + username +
    '&password='+ password +
    '&emailAddress=' + emailAddress +
  #optional params
    '&checkFree=' + checkFree +
    '&validateDNS='+ validateDNS +
    '&validateSMTP=' + validateSMTP +
    '&checkCatchAll=' + checkCatchAll +
    '&checkDisposable=' + checkDisposable

format = "JSON"
# Open the resource
buffer = open(url + '&outputFormat=' + format).read

# Parse the JSON result
result = JSON.parse(buffer)

# Print out a nice informative string
puts "JSON:\n" + result.to_yaml + "\n"

#######################
# Use an XML resource #
#######################
format = "XML"

# Open the resource
buffer = open(url + '&outputFormat=' + format).read

# Parse the XML result
result = REXML::Document.new(buffer)

# Get a few data members and make sure they aren't nil
if ((errorMessage = REXML::XPath.first(result, "/ErrorMessage/msg")) != nil)
	puts "XML:\nErrorMessage:\n\t" + errorMessage.text
else
  emailAddress = (emailAddress = REXML::XPath.first(result, "/EmailVerifyRecord/emailAddress")) == nil ? '' : emailAddress.text
  validFormat = (validFormat = REXML::XPath.first(result, "/EmailVerifyRecord/validFormat")) == nil ? '' : validFormat.text
  smtp = (smtp = REXML::XPath.first(result, "/EmailVerifyRecord/smtp")) == nil ? '' : smtp.text
  dns = (dns = REXML::XPath.first(result, "/EmailVerifyRecord/dns")) == nil ? '' : dns.text
  free = (free = REXML::XPath.first(result, "/EmailVerifyRecord/free")) == nil ? '' : free.text
  disposable = (disposable = REXML::XPath.first(result, "/EmailVerifyRecord/disposable")) == nil ? '' : disposable.text
  catchAll = (catchAll = REXML::XPath.first(result, "/EmailVerifyRecord/catchAll")) == nil ? '' : catchAll.text
  mxs = (mxs = REXML::XPath.first(result, "/EmailVerifyRecord/mxs")) == nil ? '' : mxs

  puts "XML:\n---\n" +
      " emailAddress: " + emailAddress +"\n  validFormat: " + validFormat +
      "\n  smtp: " + smtp +
      "\n  dns: " + dns +
      "\n  free: " + free +
      "\n  disposable: " + disposable +
      "\n  catchAll: " + catchAll +
      "\n  mxs:"
  mxs.each_element {
      |el| print_el = el.to_s
      print_el = print_el.gsub("<string>", "\  - ")
      print_el = print_el.gsub("</string>", "")
      puts print_el
  }

end