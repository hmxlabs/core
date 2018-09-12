ECHO "Building Core Nuget package"

msbuild HmxCore.sln /t:Clean;Rebuild /p:Configuration=Release
Nuget.exe pack core.nuspec
move *.nupkg .\Build\Output\Release