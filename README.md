# Docs

## SqlServer Password

``DockerSql2017!``

## Dependencies

``dotnet add package AspNetCore.HealthChecks.SqlServer --version 3.0.0``

## SQL Docker Hub

``docker run --name sqlserver2017v1 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=DockerSql2017!" -e "MSSQL_PID=Express" -p 11433:1433 -d microsoft/mssql-server-linux:2017-latest``
