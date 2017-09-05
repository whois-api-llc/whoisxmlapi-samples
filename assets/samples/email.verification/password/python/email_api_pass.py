try:
    from urllib.request import urlopen
except ImportError:
    from urllib2 import urlopen

emailAddress = 'support@whoisxmlapi.com';
password = 'your whois api password'
username = 'your whois api username'

url = 'http://www.whoisxmlapi.com/whoisserver/EmailVerifyService?'\
    + 'emailAddress=' + emailAddress + '&validateDNS=true&validateSMTP=true'\
    + '&checkCatchAll=true&checkFree=true&checkDisposable=true'\
    + '&username=' + username + '&password=' + password

print(urlopen(url).read().decode('utf8'))