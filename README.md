Gunslinger Templating Engine

The Gunslinger templating engine is built on top of the Handlebars templating syntax, which is an enhancement of the Mustache templating syntax. 

I'm continuing in the tradition of naming this tool after a style of facial hair, so please see the link below for examples of the gunslinger beard.

Gunslinger Beard:
https://user-images.githubusercontent.com/1778167/122961824-64603780-d34a-11eb-887b-578300dd290c.png

Use the wiki to see documentation and explanation of different elements.
https://github.com/donrolling/Gunslinger.Templates/wiki

Pluralizer
https://github.com/sarathkcm/Pluralize.NET

DOCKER
=====================================================================================================
#docker run -e "ACCEPT_EULA=Y" -e "sa_password=12345qwerASDF" -p 1433:1433 --name gunslinger-sql --hostname sql-server -d microsoft/mssql-server-linux:2019-latest
#docker run --name gunslinger-sql --hostname gunslinger-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=1StrongPwd!!" -p 1433:1433 -d microsoft/mssql-server-linux:2019-latest 

docker run --name gunslinger-sql -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=1StrongPwd!!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

docker exec -it mssql_2019 /bin/bash cd /opt/mssql-tools/bin/ ./sqlcmd -S localhost -U SA -P 'Mssql!Passw0rd' select @@version go