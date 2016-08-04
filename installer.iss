[Setup]
AppName=NecroBot
AppVersion=1.0
DefaultDirName={pf}\NecroBot
DefaultGroupName=NecroBot
UninstallDisplayIcon={app}\NecroBot.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.
OutputBaseFilename=NecroBotSetup

[Files]
Source: "*"; Excludes: "temp,Logs,installer.iss,NecroBotSetup.exe"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs      

[Icons]
Name: "{group}\NecroBot"; Filename: "{app}\NecroBot.exe"

[Dirs]
Name: "{app}\"; Permissions: everyone-modify