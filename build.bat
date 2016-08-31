echo off

nuget.exe restore "NecroBot for Pokemon Go.sln"
"C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" "NecroBot for Pokemon Go.sln" /property:Configuration=Release /property:Platform=x86