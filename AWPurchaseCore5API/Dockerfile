#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AWPurchaseCore5API/AWPurchaseCore5API.csproj", "AWPurchaseCore5API/"]
COPY ["AWPurchase.Business/AWPurchase.Business.csproj", "AWPurchase.Business/"]
COPY ["AWPurchase.DataAccess/AWPurchase.DataAccess.csproj", "AWPurchase.DataAccess/"]
RUN dotnet restore "AWPurchaseCore5API/AWPurchaseCore5API.csproj"
COPY . .
WORKDIR "/src/AWPurchaseCore5API"
RUN dotnet build "AWPurchaseCore5API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AWPurchaseCore5API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AWPurchaseCore5API.dll"]