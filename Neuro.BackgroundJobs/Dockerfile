FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Neuro.BackgroundJobs/Neuro.BackgroundJobs.csproj", "Neuro.BackgroundJobs/"]
RUN dotnet restore "Neuro.BackgroundJobs/Neuro.BackgroundJobs.csproj"
COPY . .
WORKDIR "/src/Neuro.BackgroundJobs"
RUN dotnet build "Neuro.BackgroundJobs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neuro.BackgroundJobs.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neuro.BackgroundJobs.dll"]
