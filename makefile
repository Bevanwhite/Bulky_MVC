solution:
	dotnet new sln -n ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
createGlobaljson:
	dotnet new globaljson --sdk-version ${version}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
createProject:
	dotnet new mvc -n ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
addProjectToSln:
	dotnet sln add ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
run:
	cd BulkyWeb && dotnet run
runRazor:
	cd BulkyWebRazor_Temp && dotnet run
build:
	cd BulkyWeb && dotnet build
watch:
	cd BulkyWeb && dotnet watch run
watchRazor:
	cd BulkyWebRazor_Temp && dotnet watch run
databaseUpdate:
	cd BulkyWeb && dotnet ef database update
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
databaseRazorUpdate:
	cd BulkyWebRazor_Temp && dotnet ef database update
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
addMigrations:
	cd BulkyWeb && dotnet ef migrations add ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
addRazorMigrations:
	cd BulkyWebRazor_Temp && dotnet ef migrations add ${name}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
createView:
	cd BulkyWeb && dotnet new view -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
createRazorView:
	cd BulkyWebRazor_Temp && dotnet new view -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
clean:
	cd BulkyWeb && dotnet clean
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
createClass:
	cd BulkyWeb && dotnet new class -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
createRazorClass:
	cd BulkyWebRazor_Temp && dotnet new class -n ${file} -o ${folder}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'
createRazorPage:
	cd BulkyWebRazor_Temp && dotnet new page -n ${file} -o ${folder} --namespace=${namespace}
	bash -c 'read -n1 -s -r -p "Press any key to clear..." && clear'