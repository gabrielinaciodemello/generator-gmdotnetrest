# generator-gmdotnetrest

[![npm version](https://badge.fury.io/js/generator-gmdotnetrest.svg)](https://badge.fury.io/js/generator-gmdotnetrest)

My personal Yeoman generator for .Net Core REST WebApi projects.

This generator is used to create a basic REST api project, and add new domain entities to the project from a Json file.
When new domain entity is added, the generator create all the necerary classes to manage and validate requests for /api/entity-name (GET, POST, PUT and DELETE).

The basic project generated uses:

[MongoDB C# Driver](https://github.com/mongodb/mongo-csharp-driver#readme) - For MongoDB database.

[Fluent Validation](https://github.com/JeremySkinner/FluentValidation) - For entities validation rules.

[System.IdentityModel.Tokens.Jwt](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet#readme) - For JWT Bearer authentication.

# Create new project

Install Yeoman.
- npm install yo -g


Install generator. 
- npm install generator-gmdotnetrest -g


Create new project.
- mkdir GeneratorTest
- cd GeneratorTest
- yo gmdotnetrest


Restore packages. 
- dotnet restore


Start MongoDB Server. 


Start project.
- dotnet run 
- GET for "api/users" and receive 401 Unauthorized


Create new User.
- POST for "api/users  Body = { "Name": "User", "Email": "user@user.com", "Password":"12345" }"


Do login.
- POST for "api/token Body = { "Email": "user@user.com", "Password":"12345" }" and receive a Token


Do a request with authorization header.
- GET for "api/users Header = { "Authorization": "Bearer 'Token Received' }" and receive 200


Have fun :)

# Add new domain entity

Create a Json file like this:
[
  {
    "Name": "School",
    "Properties": [
      {
        "Name": "Name",
        "Type": "string",
        "Required": [ true, "The name is required" ],
        "Length": [ 0, 10, "Name must be 10 characters or less" ]
      },
      {
        "Name": "Email",
        "Type": "string",
        "Required": [ true, "The email is required" ],
        "IsEmail": [ true, "Invalid Email" ]
      }
    ],
    "Unique": [
      [ "Email", "Name" ],"The email and name must be unique"
    ]
  },
  {
    "Name": "Car",
    "Properties": [
      {
        "Name": "Name",
        "Type": "string",
        "Required": [ true, "The name is required" ]
      },
      {
        "Name": "Register",
        "Type": "int",
        "Required": [ true, "The register is required, do you understand ?" ]
      },
    ],
    "Unique": [
      [ "Register" ],"The Register must be unique"
    ]
  }
]


In the root of your project. 
- yo gmdotnetrest:model "path of your JsonFile"


Now your new entities can be managed by 
 - api/Schools (GET, POST, PUT, DELETE)
 - api/Cars (GET, POST, PUT, DELETE)

# Future improvements 

- Refactor code.
- Add option to create new domain entity without Controller and Validation classes.
- Support FK validation rules for entities.
- Support SQL databases.
