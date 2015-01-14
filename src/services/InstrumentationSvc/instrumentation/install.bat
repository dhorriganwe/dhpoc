@echo off
md %USERPROFILE%\AppData\Roaming\npm

echo installing node-windows package...
call npm install node-windows

@echo off
echo installing Rising Tide instrumentation service...
node install.js


@echo off
set /p DUMMY=ENTER to close this window...

