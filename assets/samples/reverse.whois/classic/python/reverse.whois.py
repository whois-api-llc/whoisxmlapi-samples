try:
    from urllib.request import urlopen
except ImportError:
    from urllib2 import urlopen

term = 'wikimedia'
password = 'your whois api password'
username = 'your whois api username'

url = 'https://www.whoisxmlapi.com/reverse-whois-api/search.php?mode=preview'\
    + '&term1=' + term + '&username=' + username + '&password=' + password

print(urlopen(url).read().decode('utf8'))