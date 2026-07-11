solution:
	dotnet new sln -n ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createGlobaljson:
	dotnet new globaljson --sdk-version ${version}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createProject:
	dotnet new mvc -n ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createClassLib:
	dotnet new classlib -n ${name} 

addProjectToSln:
	dotnet sln add ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

run:
	cd BulkyWeb && dotnet run

build:
	cd BulkyWeb && dotnet build

watch:
	cd BulkyWeb && dotnet watch run

databaseUpdate:
	cd BulkyWeb && dotnet ef database update
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

addMigrations:
	cd BulkyWeb && dotnet ef migrations add ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createView:
	cd BulkyWeb && dotnet new view -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

clean:
	cd BulkyWeb && dotnet clean
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createClass:
	cd BulkyWeb && dotnet new class -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createRazorPage:
	cd BulkyWebRazor_Temp && dotnet new page -n ${file} -o ${folder} --namespace=${namespace}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'