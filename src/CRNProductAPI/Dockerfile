FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CRNProductAPI/CRNProductAPI.csproj", "src/CRNProductAPI/"]
RUN dotnet restore "src/CRNProductAPI/CRNProductAPI.csproj"
COPY . .
WORKDIR "/src/src/CRNProductAPI"
RUN dotnet publish "CRNProductAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CRNProductAPI.dll"]
