
1) set OrderProcessingSystem.Server as startup prooject
2) adjust sql server name in appsettings.json and appsettings.development.json
3) run following command to create the database
       Update-Database -Project OrderProcessingSystemInfrastructure -StartupProject OrderProcessingSystem.Server
4) select ISS from run button on top of visual studio IDE
5) run the project
5) default frontend of website will open 
6) TO SEE THE API ENDPOINTS PLEASE OPEN FOLLWING URL
    api end points can be seen and excecuted from swagger on below url
    https://localhost:44367/swagger/index.html   
7) click tryit out on swagger page=>the click try it out and click execute on /api/Order/all 
8) To run on docker change to Conatiner(DockerFile) from the debug button on top of VS IDE (Docker Desktop is required to be insatalled before running on container with enabled WASM support)
9) FrontEnd is in progress for product,custormer, order page for the website frontend which communicates to backed through api
9) Xunit for user controller is written others are in progress for backend testing and Nunit test will also be written shortly for sleninum test .
10)loginid => customer1@example.com  password=>password123
11)loginid => customer2@example.com  password=>password123
