# Sử dụng hình ảnh .NET 7 SDK để xây dựng ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 80
# Sao chép file csproj và nuget.cache vào container và thực hiện dotnet restore
COPY *.csproj .

RUN dotnet restore

# Sao chép toàn bộ mã nguồn vào container và thực hiện dotnet publish
COPY . .
RUN dotnet publish -c Release -o out

# Sử dụng hình ảnh runtime .NET 7 để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-env
WORKDIR /app
COPY --from=build-env /app/out .  
# Sao chép đầu ra từ build stage
# Môi trường biến
# ENV ASPNETCORE_ENVIRONMENT=Production
# Chạy ứng dụng khi container được khởi chạy
ENTRYPOINT ["dotnet", "WebFilm_API.dll"]
