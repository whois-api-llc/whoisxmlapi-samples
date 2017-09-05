require 'open-uri'

email = 'support@whoisxmlapi.com'
password = 'your whois api password'
username = 'your whois api username'

url = 'http://www.whoisxmlapi.com/whoisserver/EmailVerifyService?'\
    + 'emailAddress=' + email + '&username=' +username + '&password=' +password\
    + '&validateDNS=true&validateSMTP=true&checkFree=true&checkCatchAll=true'\
    + '&checkDisposable=true&outputFormat=json'

puts open(url).read