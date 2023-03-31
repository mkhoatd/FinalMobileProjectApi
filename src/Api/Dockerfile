FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/FinalMobileProjectApi/FinalMobileProjectApi.csproj", "FinalMobileProjectApi/"]
RUN dotnet restore "src/FinalMobileProjectApi/FinalMobileProjectApi.csproj"
COPY . .
WORKDIR "/src/FinalMobileProjectApi"
RUN dotnet build "FinalMobileProjectApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinalMobileProjectApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinalMobileProjectApi.dll"]
