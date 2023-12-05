FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Neuro.Api/Neuro.Api.csproj", "Neuro.Api/"]
RUN dotnet restore "Neuro.Api/Neuro.Api.csproj"
COPY . .
WORKDIR "/src/Neuro.Api"
RUN dotnet build "Neuro.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neuro.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neuro.Api.dll"]
