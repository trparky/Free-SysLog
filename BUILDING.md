## Building from Source

### Prerequisites
Install dependencies using winget:
```powershell
winget install -e --id Git.Git
winget install -e --id Microsoft.NuGet
winget install -e --id Microsoft.VisualStudio.2022.BuildTools --override "--quiet --wait --add Microsoft.VisualStudio.Workload.MSBuildTools --add Microsoft.VisualStudio.Workload.NetDesktopBuildTools --includeRecommended"
winget install -e --id Microsoft.DotNet.Framework.DeveloperPack_4 --version 4.8.1
```

### Clone the Repository
```powershell
git clone https://github.com/trparky/Free-SysLog.git
cd .\Free-SysLog\
```

### Restore Required Packages
```powershell
nuget restore ".\Free SysLog.sln"
```

### Compile Time
```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe" ".\Free SysLog.sln" /t:Rebuild /p:Configuration=Debug
& "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe" ".\Free SysLog.sln" /t:Rebuild /p:Configuration=Release
```

### Copy Images
```powershell
Copy-Item -Path '.\Free SysLog\Resources\error.png' -Destination '.\Free SysLog\bin\Release\'
Copy-Item -Path '.\Free SysLog\Resources\info.png' -Destination '.\Free SysLog\bin\Release\'
Copy-Item -Path '.\Free SysLog\Resources\warning.png' -Destination '.\Free SysLog\bin\Release\'
Copy-Item -Path '.\Free SysLog\Resources\error.png' -Destination '.\Free SysLog\bin\Debug\'
Copy-Item -Path '.\Free SysLog\Resources\info.png' -Destination '.\Free SysLog\bin\Debug\'
Copy-Item -Path '.\Free SysLog\Resources\warning.png' -Destination '.\Free SysLog\bin\Debug\'
```

### Final Notes
The compiled program is located in the Bin folder. There are two versions, Debug and Release. If running in a production environment, run the Release build.
