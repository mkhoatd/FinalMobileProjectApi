FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/", "./"]
RUN dotnet publish "./Api" -c Production --no-self-contained -o /app/publish

FROM base AS final
WORKDIR /app/publish
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
