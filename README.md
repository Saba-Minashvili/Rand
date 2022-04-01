# Rand - Clothing App
Simple clothing app created with ASP.NET Core MVC, EF Core and Microsoft SQL Server

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)

## General info
The idea was that to create a common shopping website, where people can browse clothes, add favorite ones
to the shopping cart and also buy desired ones(this logic is still being developed).

## Technologies
Project is created with:
* C#
* ASP.NET Core MVC
* Entity Framework Core
* Microsoft SQL Server
* HTML, CSS, JavaScript, Jquery, AJAX
* Bootstrap
	
## Setup
To run this project, first you need to pull the project. Then do the following step:
1) Change local database connection string, which is located in 'appsettings.json' file and then make migration only once to initialize tables.
2) You can add roles, like Admin, which gives you permission to add, create, update or delete product and have full control over them. In order to do this you need to
visit following files in project and uncomment commented code(you will see instructions above code): 
* /Views/Shared/_Navbar.cshtml, 
* /Controllers/RoleController.cs, 
* /Areas/Identity/Pages/Account/Register.cshtml,
* /Areas/Identity/Pages/Account/Register.cshtml.cs

When you finish doing all these, you can register new account as an admin or any other role you want.
3) Feel free to use application :)
