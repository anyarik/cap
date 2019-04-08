FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
WORKDIR "/src/src/Clients/CAP.Bot.Telegram"
RUN dotnet restore "CAP.Bot.Telegram.csproj"
RUN dotnet build "CAP.Bot.Telegram.csproj" --no-restore -c Release -o /app

FROM build AS publish
RUN cp -a /src/src/"Solution Items"/. /app/
RUN dotnet publish "CAP.Bot.Telegram.csproj" --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CAP.Bot.Telegram.dll"]
