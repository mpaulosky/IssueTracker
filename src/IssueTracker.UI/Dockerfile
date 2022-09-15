FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IssueTracker.UI/IssueTracker.UI.csproj", "IssueTracker.UI/"]
COPY ["src/IssueTracker.Library/IssueTracker.Library.csproj", "IssueTracker.Library/"]
RUN dotnet restore "src/IssueTracker.UI/IssueTracker.UI.csproj"
COPY . .
WORKDIR "/src/IssueTracker.UI"
RUN dotnet build "IssueTracker.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IssueTracker.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IssueTracker.UI.dll"]
