try:
    import http.client as http
except ImportError:
    import httplib as http

import json

api_key = 'Your registrant alert api key'

payload_advanced = {
    'advancedSearchTerms': [
        {
            'field': 'RegistrantContact.Name',
            'term': 'Test'
        }
    ],
    'mode': 'purchase',
    'sinceDate': '2018-07-14',
    'apiKey': api_key
}

payload_basic = {
    'basicSearchTerms': {
        'include': [
            'whois',
            'api'
        ],
        'exclude': [
            '.win'
        ]
    },
    'mode': 'purchase',
    'sinceDate': '2018-07-14',
    'apiKey': api_key
}


def print_response(txt):
    response_json = json.loads(txt)
    print(json.dumps(response_json, indent=4, sort_keys=True))


headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
}

conn = http.HTTPSConnection('registrant-alert.whoisxmlapi.com')

conn.request('POST', '/api/v2', json.dumps(payload_basic), headers)
text = conn.getresponse().read().decode('utf8')
print('Basic:')
print_response(text)

conn.request('POST', '/api/v2', json.dumps(payload_advanced), headers)
text = conn.getresponse().read().decode('utf8')
print('Advanced:')
print_response(text)
