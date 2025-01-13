# 使用 .NET 运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 使用 .NET SDK 镜像进行构建
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 复制 .csproj 文件并还原依赖项 
COPY ["CommunityBackendAPI.csproj", "./"]
RUN dotnet restore "./CommunityBackendAPI.csproj"
# 复制其余的源代码
COPY . .

# 设置工作目录并构建项目 
WORKDIR "/src"
RUN dotnet build "CommunityBackendAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 发布应用程序
FROM build AS publish 
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CommunityBackendAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 使用基于 .NET 运行时的镜像作为最终镜像
FROM base AS final
WORKDIR /app

# 从发布阶段复制文件到最终镜像
COPY --from=publish /app/publish .

# 设置容器入口点
ENTRYPOINT ["dotnet", "CommunityBackendAPI.dll"]