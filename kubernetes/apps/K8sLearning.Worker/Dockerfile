# See https://aka.ms/containerfastmode to understand how Visual Studio uses this
# Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["K8sLearning.Worker/K8sLearning.Worker.csproj", "K8sLearning.Worker/"]
RUN dotnet restore "K8sLearning.Worker/K8sLearning.Worker.csproj"
COPY . .
WORKDIR "/src/K8sLearning.Worker"
RUN dotnet build "K8sLearning.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "K8sLearning.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "K8sLearning.Worker.dll"]