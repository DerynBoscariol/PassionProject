
# Cocktail Recipe Management Application

## Setup

- Clone this repo and open in Visual Studio
- Change the target framework to 4.7.1 and then back to 4.7.2
- Create and App_data folder in the Solution Explorer
- Update the database using the Package Manage Console (update-database)
- View database (MSSQLLocalDb) using SQL Server Object Explorer

## Entities and properties

### Cocktail

int DrinkId
string DrinkName
string DrinkType
string DrinkRecipe
string LiqIn
string MixIn
BartenderId - Foreign Key

### Bartender

int BartenedrId
string FirstName
string LastName
string Email
int NumDrinks
