FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["SocketAPI/SocketAPI.csproj", "SocketAPI/"]
RUN dotnet restore "SocketAPI/SocketAPI.csproj"
COPY . .
WORKDIR "/src/SocketAPI"
RUN dotnet build "SocketAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SocketAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SocketAPI.dll"]