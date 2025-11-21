FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ApiHortifruti.csproj", "./"]
RUN dotnet restore "ApiHortifruti.csproj"

COPY . .

RUN dotnet build "ApiHortifruti.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiHortifruti.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ApiHortifruti.dll"]