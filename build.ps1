$majorVersion = 5
$minorVersion = 0
$buildVersion = 0
$revision = "unversioned"
$propsPath = "$PSScriptRoot\base.props"
$frameworkOption = ""

# validate dotnet version
try {
    [System.Version]$needVersion = "3.1"
    [System.Version]$realVersion = dotnet --version
    if ($realVersion -lt $needVersion) {
        throw "NET Core $needVersion or higher version must be installed to build this project."
    }
}
catch {
    Write-Error $_.Exception.Message
    Write-Host "Please visit the site below and install it."
    Write-Host "https://dotnet.microsoft.com/download/dotnet-core/$needVersion"
    Write-Host 'Press any key to continue...';
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
    exit 1
}

# validate mono version
try {
    if ([environment]::OSVersion.Platform -eq "Unix") {
        [System.Version]$needVersion = "6.0"
        [System.Version]$realVersion = mono --version=number
        if ($realVersion -lt $needVersion) {
            throw "mono $needVersion or higher version must be installed to build this project with .net framework 4.5."
        }
    }
}
catch {
    Write-Warning $_.Exception.Message
    Write-Warning "TargetFramework net45 skipped."
    Write-Host "If you want to build with .net framework 4.5, visit the site below and install mono."
    Write-Host "https://www.mono-project.com"
    $global:frameworkOption = "--framework netcoreapp3.1"
}

# validate .netframework 4.5
try {
    if ([environment]::OSVersion.Platform -eq "Win32NT") {
        function Test-KeyPresent([string]$path, [string]$key) {
            if (!(Test-Path $path)) { return $false }
            if ($null -eq (Get-ItemProperty $path).$key) { return $false }
            return $true
        }
        function Get-Framework-Value([string]$path, [string]$key) {
            if (!(Test-Path $path)) { return "-1" }
        
            return (Get-ItemProperty $path).$key  
        }
        if (Test-KeyPresent "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v4\Client" "Install" -or Test-KeyPresent "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v4\Full" "Install") {
            $version = Get-Framework-Value "HKLM:\Software\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
            if ($version -lt 378389) {
                throw ".net framework 4.5 or higher version must be installed to build this project"
            }
        }
    }
}
catch {
    Write-Warning $_.Exception.Message
    Write-Warning "TargetFramework net45 skipped."
    $global:frameworkOption = "--framework netcoreapp3.1"
}

# check if there are any changes in the repository.
try {
    $changes = Invoke-Expression "git status --porcelain"
    if ($changes) {
        throw "git repository has changes. build aborted."
    }
}
catch {
    Write-Error $_.Exception.Message
    exit 1
}

# get head revision of this repository
try {
    $revision = Invoke-Expression -Command "git rev-parse HEAD" 2>&1 -ErrorVariable errout
    if ($LastExitCode -ne 0) {
        throw $errout
    }
}
catch {
    Write-Warning $_.Exception.Message
    Write-Warning "revision is '$revision'"
}

# recored version to props file
$version = "$majorVersion.$minorVersion.$buildVersion-`$(TargetFramework)-$revision"
$assemblyVersion = "$majorVersion.$minorVersion"
$fileVersion = "$majorVersion.$minorVersion.$buildVersion"
[xml]$doc = Get-Content $propsPath -Encoding UTF8
foreach ($obj in $doc.Project.PropertyGroup) {        
    if ($obj.Version) {
        $obj.Version = $version
    }
    if ($obj.FileVersion) {
        $obj.FileVersion = $fileVersion
    }
    if ($obj.AssemblyVersion) {
        $obj.AssemblyVersion = $assemblyVersion
    }
}
$doc.Save($propsPath)

# build project
Invoke-Expression "dotnet build $global:frameworkOption --verbosity minimal --nologo"
if ($LastExitCode -ne 0) {
    Write-Error "build failed"
}
else {
    Write-Host "AssemblyVersion: $assemblyVersion"
    Write-Host "FileVersion: $fileVersion"
    Write-Host "revision: $revision"
}

# revert props file
Invoke-Expression "git checkout $propsPath"