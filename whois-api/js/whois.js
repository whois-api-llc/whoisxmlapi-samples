<!DOCTYPE html>
<html>
<head>
	<title>Sample Javascript API Client</title>
	<script type="text/javascript">

	window.addEventListener("load", onPageLoad, false);

	//////////////////////////
	// Fill in your details //
	//////////////////////////
	var username = "YOUR_USERNAME";
	var password = "YOUR_PASSWORD";
	var domain = "google.com";
	var jsonCallback = "LoadJSON";

	// Do something with the json result we get back
	function LoadJSON(result)
	{
		// Print out a nice informative string
		document.body.innerHTML += "<div>JSON:</div>" + RecursivePrettyPrint(result);
	}

	function onPageLoad()
	{
		/////////////////////////
		// Use a JSON resource //
		/////////////////////////
		var format = "JSON";
		var url = "http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=" + domain + "&username=" + username + "&password=" + password + "&outputFormat=" + format;
  

		// Dynamically Add a script to get our JSON data from a different server, avoiding cross origin problems
		var head = document.getElementsByTagName('head')[0];
		var script = document.createElement('script');
		script.type = 'text/javascript';
		script.src = url + "&callback=" + jsonCallback;
		head.appendChild(script);

		// The function specified in jsonCallback will be called with a single argument representing the JSON object


	}


	function RecursivePrettyPrint(obj)
	{
		var str = "";
		for (var x in obj)
		{
			if (obj.hasOwnProperty(x))
			{
				str += '<div style="margin-left: 25px;border-left:1px solid black">' + x + ": ";
				if (typeof(obj[x]) == "string")
					str += obj[x];
				else
					str += RecursivePrettyPrint(obj[x]);
				str += "</div>";
			}
		}

		return str;
	}

	</script>
</head>
<body>
</body>
</html>