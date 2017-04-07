import urllib
import json
import base64
import hmac
import hashlib
import time

username = 'username'
apiKey = 'api_key'
secret = 'secret_key'
domains = [
    'google.com',
    'example.com',
    'whoisxmlapi.com',
    'twitter.com'
]
url = 'https://whoisxmlapi.com/whoisserver/WhoisService?'
timestamp = nil
digest = nil

def generateDigest(username, timestamp, apikey, secret):
    digest = username + str(timestamp) + apikey
    hash = hmac.new(secret, digest, hashlib.md5)
    return urllib.quote(str(hash.hexdigest()))

def generateParameters(username, apikey, secret):
    timestamp = int(round(time.time() * 1000))
    digest = generateDigest(username, timestamp, apikey, secret)
    return timestamp, digest

def buildRequest(username, timestamp, digest, domain):
    requestString = "requestObject="
    data = {'u': username, 't': timestamp}
    dataJson = json.dumps(data)
    dataBase64 = base64.b64encode(dataJson)
    requestString += dataBase64
    requestString += "&digest="
    requestString += digest
    requestString += "&domainName="
    requestString += domain
    requestString += "&outputFormat=json"
    return requestString

def printResponse(response):
    responseJson = json.loads(response)
    if 'WhoisRecord' in responseJson:
        if 'contactEmail' in responseJson['WhoisRecord']:
            print "Contact Email: "
            print responseJson['WhoisRecord']['contactEmail']
        if 'createdDate' in responseJson['WhoisRecord']:
            print "Created date: "
            print responseJson['WhoisRecord']['createdDate']
        if 'expiresDate' in responseJson['WhoisRecord']:
            print "Expires date: "
            print responseJson['WhoisRecord']['expiresDate']

def request(url, username, timestamp, digest, domain):
    request = buildRequest(username, timestamp, digest, domain)
    response = urllib.urlopen(url + request).read().decode('utf8')
    return response

timestamp, digest = generateParameters(username, apiKey, secret)

for domain in domains:
    response = request(url, username, timestamp, digest, domain)
    if "Request timeout" in response:
        timestamp, digest = generateParameters(username, apiKey, secret)
        response = request(url, username, timestamp, digest, domain)
    printResponse(response)
    print "---------------------------\n"