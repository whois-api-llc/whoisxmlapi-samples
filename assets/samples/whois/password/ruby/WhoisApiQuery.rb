require 'open-uri'

domain = 'example.com'
password = 'your whois api password'
username = 'your whois api username'

url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'\
    + 'domainName=' + domain + '&username=' +username + '&password=' +password

puts open(url).read