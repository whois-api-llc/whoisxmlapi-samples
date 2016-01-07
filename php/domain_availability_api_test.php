<?php
  $username="YOUR_USERNAME";
  $password="YOUR_PASSWORD";
  $contents = file_get_contents("http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName=google.com&cmd=GET_DN_AVAILABILITY&username=$username&password=$password&outputFormat=JSON");
 // echo $contents;
  $res=json_decode($contents);
  if($res){
    if($res->ErrorMessage){
      echo $res->ErrorMessage->msg;
    } 
    else{
      $domainInfo = $res->DomainInfo;
      if($domainInfo){
        echo "Domain name: " . print_r($domainInfo->domainName,1) ."<br/>";
        echo "Domain Availability: " .print_r($domainInfo->domainAvailability,1) ."<br/>";
        //print_r($domainInfo);
      }
    }
  }  
  
  
  
?>