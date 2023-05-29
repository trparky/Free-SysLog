MSBuild.exe "Free SysLog.sln" /noconsolelogger /t:Rebuild /p:Configuration=Debug
MSBuild.exe "Free SysLog.sln" /noconsolelogger /t:Rebuild /p:Configuration=Release
if %ERRORLEVEL% neq 0 goto fail
copy "Free SysLog\bin\Release\Free SysLog.exe" "%OneDrive%\Utilities"
goto end

:fail
echo "ERROR! Compilation Failed."
:end
echo "SUCCESS! Compilation and packaging complete."
pause