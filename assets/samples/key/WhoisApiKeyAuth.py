try:
    from urllib.request import urlopen
except ImportError:
    from urllib2 import urlopen

import base64
import hashlib
import hmac
import time

domain = 'example.com'
key = 'your whois api key'
secret = 'your whois api secret key'
username = 'your whois api username'

time = int(round(time.time() * 1000))
req = base64.b64encode('{"t":' + str(time) + ',"u":"' + username + '"}')
digest = hmac.new(secret, username + str(time) + key, hashlib.md5).hexdigest()

url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'\
    + 'requestObject=' + req + '&digest=' + digest + '&domainName=' + domain

print(urlopen(url).read().decode('utf8'))