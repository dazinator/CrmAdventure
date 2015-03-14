# CrmAdventure
This repository contains code samples, and tests that explore features of the Dynamics CRM SDK.

I may often refer to code samples contained here, from my blog: http://darrelltunnell.net/

## Running the Tests
To run the tests:

    1. Adjust Config.cs with appropriate CRM URL's.
    2. Create a new User account in CRM, with a username: "testing".
    3. Add an app.config file to the project, with the password for the "testing" user account as an app setting:-
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <connectionStrings>
  </connectionStrings>
  <appSettings>
    <add key="Password" value="passwordhere"/>
  </appSettings>
</configuration>
```

You can now run all the tests.
