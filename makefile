solution:
	dotnet new sln -n Bulky
globaljson:
	dotnet new globaljson --sdk-version 8.0.422
createproject:
	dotnet new mvc -n BulkyWeb 
addProjectToSln:
	dotnet sln add BulkyWeb/BulkyWeb.csproj 

