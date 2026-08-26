solution:
	dotnet new sln -n ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createGlobaljson:
	dotnet new globaljson --sdk-version ${version}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createPackage:
	cd BulkyBookWeb && dotnet add package ${name} --version ${version}
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
	cd BulkyBookWeb && dotnet run

build:
	cd BulkyBookWeb && dotnet build

watch:
	cd BulkyBookWeb && dotnet watch run

databaseUpdate:
	dotnet ef database update \
	--project Bulky.DataAccess \
	--startup-project BulkyBookWeb
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

dropDatabase:
	dotnet ef database drop \
	--project Bulky.DataAccess \
	--startup-project BulkyBookWeb
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

addMigrations:
	dotnet ef migrations add ${name} \
	--project Bulky.DataAccess \
	--startup-project BulkyBookWeb
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createView:
	cd BulkyBookWeb && dotnet new view -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

clean:
	cd BulkyBookWeb && dotnet clean
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createClass:
	cd ${base} && dotnet new class -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createInterface:
	cd ${base} && dotnet new interface -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createRazorPage:
	cd BulkyWebRazor_Temp && dotnet new page -n ${file} -o ${folder} --namespace=${namespace}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'

createArea:
	cd BulkyBookWeb && dotnet aspnet-codegenerator area ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'