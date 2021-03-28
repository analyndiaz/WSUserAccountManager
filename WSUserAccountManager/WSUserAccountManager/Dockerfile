FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY bin/Release/net5.0/publish/ WSUserAccountManager/
WORKDIR /WSUserAccountManager
EXPOSE 8080
ENTRYPOINT ["dotnet", "WSUserAccountManager.dll"]