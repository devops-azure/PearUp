------ Below are the versions used in this framework ------
Drivers used:
 ChromeDriver                       2.33.506120
 geckodriver (Firefox)              0.18.0 
 InternetExplorerDriver server      3.8.0
 Microsoft Edge Driver
 Opera Driver

Jars used:
 selenium                           3.8.1
 junit                              4.11
 testng                             6.11
 mysql-connector-java               6.0.6
 httpclient                         4.5.3
 sqljdbc                            6.1.0
 postgresql                         9.4.1212
 
 Java Version:
 1.8
 
 Full version Updates(mm-dd-yyyy):
 
 07-08-2018 : 
  - Added TestcaseTemplate Class to utils package in main/java folder. Every new test script should extend this class to be run.
  - Added feature to open Consolidated report after each execution.
  - Added methods for handling alerts in Public library.
  
  02-04-2018:
  - Added cutomised message when email id's are not provided in Test Suite.
  
  6/7/2018:
  - Added readTextFromImageUsingOCR() method to public library. To read text from the image. Jars used : tess4j : 4.0.2
  - Added Chrome Mobile Emulation. To test the website on a specific mobile, mention mobile name after hyphen instead of chrome in the TestSuite.xls excel sheet [Exmaple: "me-Nexus 5"]