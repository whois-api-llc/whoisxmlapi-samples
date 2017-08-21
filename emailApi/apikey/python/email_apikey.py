try:
    # For Python v.3 and later
    from urllib.request import urlopen
    from urllib.parse import quote
except ImportError:
    # For Python v.2
    from urllib2 import urlopen
    from urllib2 import quote
import json
import base64
import hmac
import hashlib
import time

username = 'username'
apiKey = 'api_key'
secret = 'secret_key'

email = 'support@whoisxmlapi.com'

url = 'https://whoisxmlapi.com/whoisserver/EmailVerifyService?'
timestamp = 0
digest = 0

def generateDigest(username, timestamp, apikey, secret):
    digest = username + str(timestamp) + apikey
    hash = hmac.new(bytearray(secret.encode('utf-8')), bytearray(digest.encode('utf-8')), hashlib.md5)
    return quote(str(hash.hexdigest()))

def generateParameters(username, apikey, secret):
    timestamp = int(round(time.time() * 1000))
    digest = generateDigest(username, timestamp, apikey, secret)
    return timestamp, digest

def buildRequest(username, timestamp, digest, email):
    requestString = "requestObject="
    data = {'u': username, 't': timestamp}
    dataJson = json.dumps(data)
    dataBase64 = base64.b64encode(bytearray(dataJson.encode('utf-8')))
    requestString += dataBase64.decode('utf-8')
    requestString += '&emailAddress='
    requestString += email
    requestString += '&validateDNS=true'
    requestString += '&validateSMTP=true'
    requestString += '&checkCatchAll=true'
    requestString += '&checkFree=true'
    requestString += '&checkDisposable=true'
    requestString += '&digest='
    requestString += digest
    requestString += '&outputFormat=json'
    return requestString

def printResponse(response):
    responseJson = json.loads(response)
    print json.dumps(responseJson, indent=4, sort_keys=True)

def request(url, username, timestamp, digest, domain):
    request = buildRequest(username, timestamp, digest, email)
    response = urlopen(url + request).read().decode('utf8')
    return response

timestamp, digest = generateParameters(username, apiKey, secret)

response = request(url, username, timestamp, digest, email)
if "Request timeout" in response:
    timestamp, digest = generateParameters(username, apiKey, secret)
    response = request(url, username, timestamp, digest, email)
printResponse(response)
print("---------------------------\n")