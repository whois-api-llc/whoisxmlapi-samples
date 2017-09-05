try:
    from urllib.request import urlopen
except ImportError:
    from urllib2 import urlopen

import base64
import hashlib
import hmac
import time

domain = 'test.com'
key = 'Your whois api public key'
secret = 'Your whois api secret key'
username = 'Your whois api username'

time = int(round(time.time() * 1000))
req = base64.b64encode(
        ('{"t":' + str(time) + ',"u":"' + username + '"}').encode('ascii')
)
digest = hmac.new(
        secret.encode('ascii'), 
        (username + str(time) + key).encode('ascii'), 
        hashlib.md5
).hexdigest()

url = 'http://www.whoisxmlapi.com/whoisserver/DNSService?'
url = url + "requestObject=" + req.decode('ascii') + "&digest=" 
url = url + digest + "&domainName=" + domain
url = url + "&type=_all"

print(urlopen(url).read().decode('utf8'))