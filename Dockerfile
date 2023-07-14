FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY UsuarioService.sln ./
COPY app/app.csproj ./app/
COPY dominio/dominio.csproj ./dominio/
COPY repositorio/repositorio.csproj ./repositorio/
COPY service/service.csproj ./service/
COPY test/test.csproj ./test/

RUN dotnet restore

COPY . ./

RUN dotnet build -c Release

RUN dotnet publish app/app.csproj -c Release -o /app/out
RUN dotnet publish service/service.csproj -c Release -o /app/out
RUN dotnet publish repositorio/repositorio.csproj -c Release -o /app/out
RUN dotnet publish dominio/dominio.csproj -c Release -o /app/out
RUN dotnet publish test/test.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "app.dll"]
