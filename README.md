# projeto_forest

Projeto Livre - WebAPI com Asp.NET Core 3.1 Autenticação JWT e Swagger

#### Instruções
    1- dotnet restore
    2- dotnet add package Microsoft.AspNetCore.Authentication --version 2.2.0
    3- dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.1.7
    4- dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 3.1.5
    5- dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 3.1.5
    6- dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 8.0.1
    7- dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.7
    8- dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 3.1.7
    9- dotnet add package Swashbuckle.AspNetCore --version 5.5.1
    10- Configuração de banco de dados em appsettings.json
    11- Executar para gerar os Migrations: dotnet ef migrations add InitialCreate
    12- Executar para atualizar o banco de dados: dotnet ef database update
    13- Executar no banco de daos: 
        INSERT INTO AspNetRoles (Name, NormalizedName) 
        VALUES ('ADMINISTRADOR', 'ADMINISTRADOR')
    14- Executar: dotnet run ou dotnet watch run
    
