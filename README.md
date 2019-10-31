# Cricetidae
The bonus data retriever for the Cricetidae supermarket. Run the UpdateData to get all the data you need.
Run the api and frontend docker containers

### Requirements
 - Dotnet core 3
 - Sql server 
 
### Running dev
Well first you have to init dotnet user-secrets in you api project and register a ConnectionStrings__DefaultConnection with the connection string of your db
Second run it either in iis or as docker locally
Lastly run the frontend with npm run server
 
### Todo 
 - Rewrite Netwonsoft.Json to System.Text.Json
 - Clean up api docker
 - Improve the readme :D