require 'open-uri'

domain = 'example.com'
password = 'Your whois api password'
username = 'Your whois api username'

url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'\
    + 'domainName=' + domain + '&username=' +username + '&password=' +password\
    + "&cmd=GET_DN_AVAILABILITY" + "&getMode=DNS_AND_WHOIS"

puts open(url).read