<?php
  $username="YOUR_USERNAME";
  $password="YOUR_PASSWORD";	
  $contents = file_get_contents("http://www.whoisxmlapi.com//whoisserver/WhoisService?domainName=google.com&username=$username&password=$password&outputFormat=JSON");
  //echo $contents;
  $res=json_decode($contents);
  if($res){
  	if($res->ErrorMessage){
  		echo $res->ErrorMessage->msg;
  	}	
  	else{
  		$whoisRecord = $res->WhoisRecord;
  		if($whoisRecord){
    		echo "Domain name: " . print_r($whoisRecord->domainName,1) ."<br/>";
    		echo "Created date: " .print_r($whoisRecord->createdDate,1) ."<br/>";
    		echo "Updated date: " .print_r($whoisRecord->updatedDate,1) ."<br/>";
    		if($whoisRecord->registrant)echo "Registrant: <br/><pre>" . print_r($whoisRecord->registrant->rawText, 1) ."</pre>";
    		//print_r($whoisRecord);
  		}
  	}
  }
  
?>