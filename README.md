## Suculentas da Rô - Servidor

### Gerar release:
-- dotnet publish -c Release -o .\Suculentas.Publish

### Gerar Migration e DataBase
-- cd .\Suculentas.Repository\
-- dotnet ef --startup-project ..\Suculentas.WebApi\  migrations add init
-- dotnet ef --startup-project ..\Suculentas.WebApi\ database update