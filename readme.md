_May' da cai' docker

+docker run -p 3307:3306 --name mysql -v "path":/var/lib/mysql -e MYSQL_ROOT_PASSWORD=root -d mysql:5.7

+make sure that your folder in path is empty

+dir to Project.Data

_May' da cai' .net

+dotnet ef migrations add dockertest

+dotnet ef database update

+dir to Project.BackendApi

+dotnet run

server chay o localhost:5001


_______________


- check database:
+ docker exec -it id bash
+ mysql -u root -proot


________________

app.setting

chinh trong data va' backend

{
  "ConnectionStrings": {
    "ProjectDb": "Server=localhost;Port=3307;User=root;password=root;database=Project;"
  }
}
