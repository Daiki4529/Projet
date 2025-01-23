# Netflix Titles Data Importer

## Description

This project imports Netflix titles data from a CSV file into a MongoDB database. The data includes various details about Netflix titles such as show ID, type, title, director, cast, country, date added, release year, rating, duration, listed categories, and description.

## Dataset

The dataset used in this project is from Kaggle: [Netflix Movies and TV Shows](https://www.kaggle.com/datasets/anandshaw2001/netflix-movies-and-tv-shows/)

## Project Structure
```
.gitignore
assets/
    datasets/
        mock_netflix_titles.csv
        netflix_titles.csv
bin/
    Debug/
        net6.0/
            CsvHelper.dll
            DnsClient.dll
            Microsoft.Extensions.Logging.Abstractions.dll
            MongoDB.Bson.dll
            MongoDB.Driver.dll
            Projet.deps.json
            Projet.dll
            Projet.exe
            ...
NetflixTitle.cs
obj/
    Debug/
        net6.0/
            ...
project.assets.json
Program.cs
Projet.csproj
Projet.sln
README.md
```
## Prerequisites
- .NET 6.0 SDK
- MongoDB server running locally on mongodb://localhost:27017

## Setup
1. Clone the repository:
```bash
git clone <repository-url>
cd <repository-directory>
```
2. Restore the dependencies:
```bash
dotnet restore
```
3. Build the projet :
```bash
dotnet build
```
4. Ensure MongoDB is running locally.
5. Place the [netflix_titles.csv](./assets/datasets/netflix_titles.csv) file in the [datasets](./assets/datasets/) directory.

## Running the Project
To run the project and import the data into MongoDB, execute the following command:
```bash
dotnet run
```

## Code Explanation
- [Program.cs](./Program.cs): Contains the main logic to read the CSV file and import the data into MongoDB.
- [NetflixTitle.cs](./NetflixTitle.cs): Defines the NetflixTitle class with properties corresponding to the CSV columns.

## Notes
- All fields in the CSV are nullable except for show_id.
- The project uses CsvHelper for reading the CSV file and MongoDB.Driver for interacting with MongoDB.

## License
This project is licensed under the MIT License
