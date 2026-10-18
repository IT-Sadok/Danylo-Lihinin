FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY WebApiBooking/WebApiBooking.csproj WebApiBooking/
COPY WebApiBooking.Application/WebApiBooking.Application.csproj WebApiBooking.Application/
COPY WebApiBooking.Domain/WebApiBooking.Domain.csproj WebApiBooking.Domain/
COPY WebApiBooking.Infrastructure/WebApiBooking.Infrastructure.csproj WebApiBooking.Infrastructure/
RUN dotnet restore WebApiBooking/WebApiBooking.csproj

COPY . .
RUN dotnet publish WebApiBooking/WebApiBooking.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApiBooking.dll"]