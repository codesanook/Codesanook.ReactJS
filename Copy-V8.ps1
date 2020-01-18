param(
    [Parameter(Mandatory = $true)] [string] $SolutionDir,
    [Parameter(Mandatory = $true)] [string] $TargetDir
)
"SolutionDir $SolutionDir"
"TargetDir $TargetDir"

# https://github.com/projectkudu/kudu/issues/2048
$WarningPreference = "SilentlyContinue"

$destinationDir = Join-Path -Path $SolutionDir -ChildPath "Orchard.Web/bin/x86"
"destinationDir $destinationDir"
New-Item -ItemType Directory $destinationDir -Force -ErrorAction SilentlyContinue | Out-Null

$files = Get-ChildItem -Path $TargetDir -Recurse | Where-Object { $_.FullName -Match "x86.*v8" } 
$files | Copy-Item -Destination $destinationDir -Force -Verbose