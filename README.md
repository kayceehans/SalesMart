# SalesMart
## Purpose of the application  
It is an API developed with .NET 8.0 for managing sales orders, products, and 
provide a dashboard for analysing sales data by providing most sold and most expensive product.


## How to setup and run the application

Take these steps to get and run the application:
1. Clone the project from the repo, there are 2 Branches, any of the Branch (Master/Development) as they are both in sync.
2. The project is structured for DataBase First approach, Hence, the Database script will make it easier to get started. 
3. Run the SalesMart Database script on your SQL Server and ensure all the tables were created without errors.
4. Update the ConnectionString to align with your SQL Server path where the script were ran.
5. Build, Clean and Rebuild the project to ensure all dependencies are resolved with out errors.
6. Most importantly, please note that all endpoints request/response requires a valid clientID/API keys.
7. For further clarification and urgent support, kindly reach out to: kazeem.hassan@outlook.com

## Additional considerations or dependencies
It is required to set-up a client to receive the SignalR sales updates.
Also, the API Key is required to integrate with the API.
It is docker enable as you may require a docker desktop on your machine.

## Assumption Made 
1. There is a Front End that integrate with the API sending request carrying AuthenticationHeader with API Key and Token.
2. Requests are classified as coming from 2 user profiles namely:

  A. Customers: Can view products, Sign-Up and Login
  B. Admin : Can do all including Deleting Products and Sales order.
3. There is a front end library/framework/Mobile that receives the SignalR broadcast
