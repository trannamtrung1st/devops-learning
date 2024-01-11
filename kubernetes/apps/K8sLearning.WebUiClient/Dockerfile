FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["K8sLearning.WebUiClient/K8sLearning.WebUiClient.csproj", "K8sLearning.WebUiClient/"]
RUN dotnet restore "K8sLearning.WebUiClient/K8sLearning.WebUiClient.csproj"
COPY . .
WORKDIR "/src/K8sLearning.WebUiClient"
RUN dotnet build "K8sLearning.WebUiClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "K8sLearning.WebUiClient.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY K8sLearning.WebUiClient/nginx.conf /etc/nginx/nginx.conf
COPY K8sLearning.WebUiClient/scripts/startup.sh /init/startup.sh
CMD /bin/sh /init/startup.sh
