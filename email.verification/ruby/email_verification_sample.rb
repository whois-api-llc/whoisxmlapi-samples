require 'erb'
require 'json'
require 'net/https'
require 'rexml/document'
require 'rexml/xpath'
require 'yaml'

########################
# Fill in your details #
########################
api_key = 'Your email verification api key'

email_address = 'support@whoisxmlapi.com'

url = 'https://emailverification.whoisxmlapi.com/api/v1' \
      '?apiKey=' + ERB::Util.url_encode(api_key) +
      '&emailAddress=' + ERB::Util.url_encode(email_address) +
      '&outputFormat='

#########################
# Use the JSON resource #
#########################
format = 'JSON'

# Open the resource
buffer = Net::HTTP.get(URI.parse(url + format))

# Parse the JSON result
result = JSON.parse(buffer)

# Print out a nice informative string
puts "JSON:\n" + result.to_yaml + "\n"

########################
# Use the XML resource #
########################
format = 'XML'

# Open the resource
buffer = Net::HTTP.get(URI.parse(url + format))

# Parse the XML result
result = REXML::Document.new(buffer)

# Get a few data elements and make sure they aren't nil
if !(error_message = REXML::XPath.first(result, '/ErrorMessage/msg')).nil?
  puts "XML:\nErrorMessage:\n\t" + error_message.text
else
  el_all = '/EmailVerifyRecord/catchAllCheck'
  el_disp = '/EmailVerifyRecord/disposableCheck'
  el_dns = '/EmailVerifyRecord/dnsCheck'
  el_fmt = '/EmailVerifyRecord/formatCheck'
  el_free = '/EmailVerifyRecord/freeCheck'
  el_mail = '/EmailVerifyRecord/emailAddress'
  el_mxs = '/EmailVerifyRecord/mxRecords'
  el_smtp = '/EmailVerifyRecord/smtpCheck'

  all = (all = REXML::XPath.first(result, el_all)).nil? ? '' : all.text
  disp = (disp = REXML::XPath.first(result, el_disp)).nil? ? '' : disp.text
  dns = (dns = REXML::XPath.first(result, el_dns)).nil? ? '' : dns.text
  fmt = (fmt = REXML::XPath.first(result, el_fmt)).nil? ? '' : fmt.text
  free = (free = REXML::XPath.first(result, el_free)).nil? ? '' : free.text
  mail = (mail = REXML::XPath.first(result, el_mail)).nil? ? '' : mail.text
  mxs = (mxs = REXML::XPath.first(result, el_mxs)).nil? ? [] : mxs
  smtp = (smtp = REXML::XPath.first(result, el_smtp)).nil? ? '' : smtp.text

  puts "XML:\n---\n" \
       ' emailAddress: ' + mail +
       "\n  validFormat: " + fmt +
       "\n  smtp: " + smtp +
       "\n  dns: " + dns +
       "\n  free: " + free +
       "\n  disposable: " + disp +
       "\n  catchAll: " + all +
       "\n  mxs:"

  mxs.each do |el|
    print_el = el.to_s
    next unless print_el.include? 'mxRecord'
    print_el = print_el.gsub('<mxRecord>', '    - ')
    print_el = print_el.gsub('</mxRecord>', '')
    puts print_el
  end

end