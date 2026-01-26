@echo off

setlocal EnableDelayedExpansion

set "test=F"

FOR %%i in (*) do (

set "test=F"

if "%%i" == "Arction.Licensing.dll" set test=T
if "%%i" == "Arction.DirectX.dll" set test=T
if "%%i" == "WpfToolkit.dll" set test=T
if "%%i" == "cleanup.bat" set test=T

if "!test!" == "F" del %%i

)

endlocal