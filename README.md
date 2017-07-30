# Game Of Drones
###### Test App for UruIT

## Info
This solution contains four projects
1. Database project *[GameOfDrones.Database]* : Contains the db structure
2. Models *[GameOfDrones.Entity]* : Contains classes and some common logic
3. Web Site *[GameOfDrones.Site]* : Contains the client's logic of the game
4. Web Api *[GameOfDrones.WebApi]* : Contains the API for handling the game's logic

## Setup
### Deploy Database Project
This will create the database and the user of the application
**GameOfDrones** is the default name of the database
## Deploy the Sites
If you're using your localhost as server
1. WebApi as **http://localhost/gameofdrones/api** *(default*)*
2. Web Site as **http://localhost/gameofdrones.site/** *(default*)*
## Hints*
If you change the WebApi's address, you'll need to change the following file:
- [x] GameOfDrones.Site/Scripts/app/config.js the field **apiUrl**

If you change the name of the database, you'll need to change the following file:

- [x] GameOfDrones.WebApi/Web.config the node **connectionStrings** 
