import urllib.request
import json
import xml.etree.ElementTree as etree

########################
# Fill in your details #
########################
username = "YOUR_USERNAME"
password = "YOUR_PASSWORD"
domain = "google.com"

# A function to recursively print out multi-level dicts with indentation
def RecursivePrettyPrint(obj, indent):
    for x in list(obj):
        if isinstance(obj[x], dict):
            print (' '*indent + str(x)[0:50] + ": ")
            RecursivePrettyPrint(obj[x], indent + 5)
        elif isinstance(obj[x], list):
            print(' '*indent + str(x)[0:50] + ": " + str(list(obj[x])))
        else:
            print (' '*indent + str(x)[0:50] + ": " + str(obj[x])[0:50].replace("\n",""))
            
#######################
# Use a JSON resource #
#######################
format = "JSON"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format

# Get and build the JSON object
result = json.loads(urllib.request.urlopen(url).readall().decode('utf8'))

# Handle some odd JS cases for audit, whose properties are named '$' and '@class'.  Dispose of '@class' and just make '$' the value for each property
if 'audit' in result:
	if 'createdDate' in result['audit']:
		if '$' in result['audit']['createdDate']:
			result['audit']['createdDate'] = js['audit']['createdDate']['$']
	if 'updatedDate' in result['audit']:
		if '$' in result['audit']['updatedDate']:
			result['audit']['updatedDate'] = js['audit']['updatedDate']['$']

# Get a few data members.
if ('WhoisRecord' in result):
    registrantName = result['WhoisRecord']['registrant']['name']
    domainName = result['WhoisRecord']['domainName']
    createdDate = result['WhoisRecord']['createdDate']

# Print out a nice informative string
RecursivePrettyPrint(result, 0)
			
#######################
# Use an XML resource #
#######################
format = "XML"
url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=' + domain + '&username=' + username + '&password=' + password + '&outputFormat=' + format
result = etree.parse(urllib.request.urlopen(url))
root = result.getroot()

# A function to recursively build a dict out of an ElementTree
def etree_to_dict(t):
    if (len(list(t)) == 0):
        d = t.text
    else:
        d = {}
        for node in list(t):
            d[node.tag] = etree_to_dict(node)
            if isinstance(d[node.tag], dict):
                d[node.tag] = d[node.tag];
    return d

# Create the dict with the above function.
result = {root.tag :etree_to_dict(root)}

# Get a few data members.
if ('WhoisRecord' in result):
    registrantName = result['WhoisRecord']['registrant']['name']
    domainName = result['WhoisRecord']['domainName']
    createdDate = result['WhoisRecord']['createdDate']

# Print out a nice informative string
#print ("'" + registrantName + "' created " + domainName + " on " + createdDate)
RecursivePrettyPrint(result, 0)
