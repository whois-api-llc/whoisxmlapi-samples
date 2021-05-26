try:
    # For Python v.3 and later
    from urllib.request import urlopen, pathname2url
except ImportError:
    # For Python v.2
    from urllib import pathname2url
    from urllib2 import urlopen

import json
import xml.etree.ElementTree as ETree

########################
# Fill in your details #
########################
api_key = 'Your Whois API key'
domain = 'whoisxmlapi.com'

api_url = 'https://www.whoisxmlapi.com/whoisserver/WhoisService'

url = api_url \
      + '?domainName=' + pathname2url(domain) \
      + '&apiKey=' + pathname2url(api_key) \
      + '&outputFormat='


# A function to recursively build a dict out of an ElementTree
def etree_to_dict(t):
    if len(list(t)) == 0:
        d = t.text
    else:
        d = {}
        for node in list(t):
            d[node.tag] = etree_to_dict(node)
            if isinstance(d[node.tag], dict):
                d[node.tag] = d[node.tag]
    return d


# A function to recursively print out multi-level dicts with indentation
def recursive_pretty_print(obj, indent):
    for x in list(obj):
        if isinstance(obj[x], dict):
            print (' ' * indent + str(x)[0:50] + ': ')
            recursive_pretty_print(obj[x], indent + 5)
        elif isinstance(obj[x], list):
            print(' ' * indent + str(x)[0:50] + ': ' + str(list(obj[x])))
        else:
            print(' ' * indent + str(x)[0:50] + ': '
                  + str(obj[x])[0:50].replace("\n", ""))


#######################
# Use the JSON resource #
#######################
output_format = 'JSON'
uri = url + output_format

# Get and build the JSON object
result = json.loads(urlopen(uri).read().decode('utf8'))

# Handle some odd JS cases for audit, whose properties are named '$' and
# '@class'.  Dispose of '@class' and just make '$' the value for each property
if 'audit' in result:
    if 'createdDate' in result['audit']:
        if '$' in result['audit']['createdDate']:
            result['audit']['createdDate'] =\
                result['audit']['createdDate']['$']
    if 'updatedDate' in result['audit']:
        if '$' in result['audit']['updatedDate']:
            result['audit']['updatedDate'] =\
                result['audit']['updatedDate']['$']

# Get a few data members.
if 'WhoisRecord' in result:
    registrantName = result['WhoisRecord']['registrant']['name']
    domainName = result['WhoisRecord']['domainName']
    createdDate = result['WhoisRecord']['createdDate']

# Print out a nice informative string
recursive_pretty_print(result, 0)

########################
# Use the XML resource #
########################
output_format = 'XML'
uri = url + output_format

result = ETree.parse(urlopen(uri))
root = result.getroot()

# Create the dict with the above function.
result = {
    root.tag: etree_to_dict(root)
}

# Get a few data members.
if 'WhoisRecord' in result:
    registrantName = result['WhoisRecord']['registrant']['name']
    domainName = result['WhoisRecord']['domainName']
    createdDate = result['WhoisRecord']['createdDate']

# Print out a nice informative string
recursive_pretty_print(result, 0)
