FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["RealState.Statistics.Web/RealState.Statistics.Web.csproj", "RealState.Statistics.Web/"]
COPY ["RealState.ReadStack/RealState.ReadStack.csproj", "RealState.ReadStack/"]
COPY ["RealState.Models/RealState.Models.csproj", "RealState.Models/"]
COPY ["APIConsumer/APIConsumer.csproj", "APIConsumer/"]
RUN dotnet restore "RealState.Statistics.Web/RealState.Statistics.Web.csproj"
COPY . .
WORKDIR "/src/RealState.Statistics.Web"
RUN dotnet build "RealState.Statistics.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RealState.Statistics.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RealState.Statistics.Web.dll"]