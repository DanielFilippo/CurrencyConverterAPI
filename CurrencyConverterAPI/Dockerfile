#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CurrencyConverterAPI/CurrencyConverterAPI.csproj", "CurrencyConverterAPI/"]
RUN dotnet restore "CurrencyConverterAPI/CurrencyConverterAPI.csproj"
COPY ./CurrencyConverterAPI ./CurrencyConverterAPI
WORKDIR "/src/CurrencyConverterAPI"
RUN dotnet build "CurrencyConverterAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyConverterAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN useradd -m myappuser
USER myappuser

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet ./CurrencyConverterAPI.dll	
