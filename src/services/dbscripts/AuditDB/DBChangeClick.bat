@echo off
del DBScriptError.log

if "%1" == "" (
set /p DBServer= Enter database server:
) else (set DBServer=%1)
@echo\%DBServer%


"C:\Program Files\PostgreSQL\9.3\bin\psql.exe" -h %DBServer% -U postgres -f DBScript.sql 2>DBScriptError.log

   GOTO Err%ERRORLEVEL%
   :err0
   GOTO End
   :err1
   GOTO End
   :err2
   Echo "Unable to connect to specified host trying without host flag"
   "C:\Program Files\PostgreSQL\9.3\bin\psql.exe" -U postgres -f DBScript.sql 2>DBScriptError.log
   
:End

copy DBScriptError.log CON
