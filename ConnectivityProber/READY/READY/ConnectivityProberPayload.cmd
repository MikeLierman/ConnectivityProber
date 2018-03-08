@echo off
if exist "C:\IT\ConnectivityProber.exe" (
    cd "C:\IT"
    ConnectivityProber.exe
) else (
    powershell -Command "(New-Object Net.WebClient).DownloadFile('http://YOURSERVER.net/dl/ConnectivityProber/ConnectivityProber.exe', 'ConnectivityProber.exe')"

    if not exist "C:\IT" md "C:\IT" > nul  
    move ConnectivityProber.exe "C:\IT\ConnectivityProber.exe" > nul
    cd "C:\IT"
    ConnectivityProber.exe
)