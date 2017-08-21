try:
    from urllib.request import urlopen
except ImportError:
    from urllib2 import urlopen

emailAddress = 'support@whoisxmlapi.com';
password = 'your_whois_api_password'
username = 'your_whois_api_username'

url = 'http://www.whoisxmlapi.com/whoisserver/EmailVerifyService?'\
    + 'emailAddress=' + emailAddress + '&validateDNS=true&validateSMTP=true'\
    + '&checkCatchAll=true&checkFree=true&checkDisposable=true'\
    + '&username=' + username + '&password=' + password

print(urlopen(url).read().decode('utf8'))