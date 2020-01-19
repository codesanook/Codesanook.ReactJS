param(
    [Parameter(Mandatory = $true)] [string] $TargetDir,
    [Parameter(Mandatory = $true)] [string] $WebBinDir
)

$ErrorActionPreference = "Continue"
$VerbosePreference = "Continue"
$WarningPreference = "Continue"
$ProgressPreference = "SilentlyContinue"

"Current dir $PSScriptRoot"
"TargetDir $TargetDir"
"WebBinDir $WebBinDir"

return

# https://github.com/projectkudu/kudu/issues/2048

$destinationDir = Join-Path -Path "$WebBinDir" -ChildPath "x86"
"Creating $destinationDir directory if not exit"
New-Item -ItemType Directory -Path "$destinationDir" -Force -ErrorAction Continue

$files = Get-ChildItem -Path "$TargetDir" -Recurse | Where-Object { "$($_.FullName)" -Match "x86.*v8" } 

"Copying $($files.Length) files to $destinationDir directory"
$files | ForEach-Object {
   "Copying $($_.FullName) file to $destinationDir directory"
    Copy-Item -Path $($_.FullName) -Destination "$destinationDir" -Force -ErrorAction Continue
}