#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IPFees.Web/IPFees.Web.csproj", "IPFees.Web/"]
COPY ["IPFees.Core/IPFees.Core.csproj", "IPFees.Core/"]
RUN dotnet restore "IPFees.Web/IPFees.Web.csproj"
COPY . .
WORKDIR "/src/IPFees.Web"
RUN dotnet build "IPFees.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IPFees.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IPFees.Web.dll"]