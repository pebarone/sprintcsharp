@echo off
REM Script para rodar o container com a connection string como variável de ambiente

echo ========================================
echo Rodando container com configuracao Oracle
echo ========================================
echo.

docker run --rm -p 5000:8080 ^
  --name sprintcsharp_test ^
  -e "ConnectionStrings__OracleConnection=User Id=RM99781;Password=270904;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)))" ^
  pbrnx/sprint-csharp:latest

echo.
echo Se funcionou, acesse: http://localhost:5000/swagger
echo.
pause
