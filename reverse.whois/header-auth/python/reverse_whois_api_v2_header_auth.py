try:
    import http.client as http
except ImportError:
    import httplib as http

import json

api_key = 'Your reverse whois api key'


def print_response(txt):
    response_json = json.loads(txt)
    print(json.dumps(response_json, indent=4, sort_keys=True))


payload_advanced = {
    'advancedSearchTerms': [
        {
            'field': 'RegistrantContact.Name',
            'term': 'Test'
        }
    ],
    'searchType': 'current',
    'sinceDate': '2018-07-18',
    'mode': 'purchase',
    'responseFormat': 'json'
}

payload_basic = {
    'basicSearchTerms': {
        'include': [
            'test',
            'US'
        ],
        'exclude': [
            'Europe',
            'EU'
        ],
    },
    'searchType': 'current',
    'sinceDate': '2018-07-18',
    'mode': 'purchase',
    'responseFormat': 'json'
}

headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'X-Authentication-Token': api_key
}

conn = http.HTTPSConnection('reverse-whois-api.whoisxmlapi.com')

# Basic search

conn.request('POST', '/api/v2', json.dumps(payload_basic), headers)

response = conn.getresponse()
text = response.read().decode('utf8')

print('Basic:')
print_response(text)

# Advanced search

conn.request('POST', '/api/v2', json.dumps(payload_advanced), headers)

response = conn.getresponse()
text = response.read().decode('utf8')

print('Advanced:')
print_response(text)
