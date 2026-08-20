# ---------- 1. AŞAMA: Derleme (Build) ----------
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY CartMVCApp.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ---------- 2. AŞAMA: Çalıştırma (Runtime) ----------
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render.com PORT ortam değişkenini otomatik atar (Program.cs bunu okur)
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CartMVCApp.dll"]
