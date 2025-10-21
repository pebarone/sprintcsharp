# ---- STAGE 1: Build ----
# Usamos a imagem completa do SDK para compilar seu projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto (.sln e .csproj)
# Fazer isso primeiro nos permite aproveitar o cache do Docker
COPY *.sln .
COPY sprintcsharp/*.csproj ./sprintcsharp/

# Restaura todos os pacotes (EF, Swagger, etc.)
RUN dotnet restore

# Copia todo o resto do código-fonte
COPY . .

# Publica o aplicativo para produção (versão otimizada)
RUN dotnet publish "sprintcsharp/sprintcsharp.csproj" -c Release -o /app/publish

# ---- STAGE 2: Final Image ----
# Usamos a imagem de runtime do ASP.NET, que é muito menor e mais segura
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# O .NET 8 em containers usa a porta 8080 por padrão
EXPOSE 8080

# Define a variável de ambiente para a porta
ENV ASPNETCORE_URLS=http://+:8080

# Define o comando que será executado quando o container iniciar
ENTRYPOINT ["dotnet", "sprintcsharp.dll"]