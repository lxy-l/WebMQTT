#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["WebMQTT/WebMQTT.csproj", "WebMQTT/"]
#COPY ["MQTT/MQTT.csproj", "MQTT/"]
#RUN dotnet restore "WebMQTT/WebMQTT.csproj"
#COPY . .
#WORKDIR "/src/WebMQTT"
#RUN dotnet build "WebMQTT.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "WebMQTT.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WebMQTT.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY . .
ENTRYPOINT ["dotnet", "WebMQTT.dll"]