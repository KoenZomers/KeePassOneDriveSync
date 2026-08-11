param(
	[string] $Configuration = 'Release',
	[switch] $SkipBuild,
	[switch] $CopyToKeePassPluginFolder
)

$ErrorActionPreference = 'Stop'

$pluginName = 'KeeOneDriveSync'
$sourceFolderName = 'KoenZomers.KeePass.OneDriveSync'
$root = $PSScriptRoot
$solutionPath = Join-Path $root 'KoenZomers.KeePass.OneDriveSync.sln'
$sourceFolder = Join-Path $root $sourceFolderName
$projectFileName = 'KoenZomers.KeePass.OneDriveSync.csproj'
$projectPath = Join-Path $sourceFolder $projectFileName
$configuration = $Configuration
$outputFolder = Join-Path $sourceFolder "bin\$configuration"
$stageRoot = Join-Path $root 'obj\plgx'
$stageSourceFolder = Join-Path $stageRoot $sourceFolderName
$stageProjectPath = Join-Path $stageSourceFolder $projectFileName
$stageReferencesFolderName = 'PlgxReferences'
$stageReferencesFolder = Join-Path $stageSourceFolder $stageReferencesFolderName

function Get-PluginVersion {
	$assemblyInfoPath = Join-Path $sourceFolder 'Properties\AssemblyInfo.cs'
	$assemblyVersionLine = Get-Content -Path $assemblyInfoPath |
		Where-Object { $_ -match '^\s*\[assembly:\s*AssemblyVersion\("([^"]+)"\)\]' } |
		Select-Object -First 1
	if (!$assemblyVersionLine -or $assemblyVersionLine -notmatch '^\s*\[assembly:\s*AssemblyVersion\("([^"]+)"\)\]') {
		throw "Unable to determine plugin version from '$assemblyInfoPath'."
	}

	return $Matches[1]
}

function Get-MSBuildPath {
	$vsWhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
	if (Test-Path $vsWhere) {
		$instances = & $vsWhere -all -products * -requires Microsoft.Component.MSBuild -format json | ConvertFrom-Json
		$instance = $instances |
			Where-Object { $_.productId -notlike '*.BuildTools' } |
			Sort-Object installationVersion -Descending |
			Select-Object -First 1

		if (!$instance) {
			$instance = $instances |
				Sort-Object installationVersion -Descending |
				Select-Object -First 1
		}

		if ($instance) {
			$msBuild = Join-Path $instance.installationPath 'MSBuild\Current\Bin\MSBuild.exe'
			if (Test-Path $msBuild) {
				return $msBuild
			}
		}
	}

	$command = Get-Command msbuild.exe -ErrorAction SilentlyContinue
	if ($command) {
		return $command.Source
	}

	throw 'MSBuild.exe could not be found. Install Visual Studio Build Tools or run this script from a Developer Command Prompt.'
}

function Get-KeePassPath {
	if ($env:KEEPASS_EXE -and (Test-Path $env:KEEPASS_EXE)) {
		return $env:KEEPASS_EXE
	}

	$defaultKeePassPaths = @(
		$(if ($env:ProgramW6432) { Join-Path $env:ProgramW6432 'KeePass Password Safe 2\KeePass.exe' }),
		$(if ($env:ProgramFiles) { Join-Path $env:ProgramFiles 'KeePass Password Safe 2\KeePass.exe' }),
		$(if (${env:ProgramFiles(x86)}) { Join-Path ${env:ProgramFiles(x86)} 'KeePass Password Safe 2\KeePass.exe' })
	) | Where-Object { $_ }

	foreach ($defaultKeePassPath in $defaultKeePassPaths) {
		if (Test-Path $defaultKeePassPath) {
			return $defaultKeePassPath
		}
	}

	throw 'KeePass.exe could not be found. Set the KEEPASS_EXE environment variable to the KeePass.exe path.'
}

function Copy-SourceTree {
	if (Test-Path $stageRoot) {
		Remove-Item $stageRoot -Recurse -Force
	}

	New-Item -ItemType Directory -Path $stageRoot | Out-Null
	$robocopyArgs = @(
		$sourceFolder,
		$stageSourceFolder,
		'/E',
		'/XD', 'bin', 'obj', 'Packages',
		'/XF', '*.csproj.user'
	)

	& robocopy @robocopyArgs | Out-Host
	if ($LASTEXITCODE -gt 7) {
		throw "Robocopy failed with exit code $LASTEXITCODE."
	}

	$global:LASTEXITCODE = 0
}

function Add-PlgxReference {
	param(
		[xml] $ProjectXml,
		[System.Xml.XmlElement] $ItemGroup,
		[string] $AssemblyName,
		[string] $HintPath
	)

	$namespaceUri = $ProjectXml.Project.NamespaceURI
	$referenceElement = $ProjectXml.CreateElement('Reference', $namespaceUri)
	$referenceElement.SetAttribute('Include', $AssemblyName)

	$hintPathElement = $ProjectXml.CreateElement('HintPath', $namespaceUri)
	$hintPathElement.InnerText = $HintPath
	[void] $referenceElement.AppendChild($hintPathElement)

	$privateElement = $ProjectXml.CreateElement('Private', $namespaceUri)
	$privateElement.InnerText = 'True'
	[void] $referenceElement.AppendChild($privateElement)

	[void] $ItemGroup.AppendChild($referenceElement)
}

function Add-PlgxReferencesToProject {
	$referenceDlls = Get-ChildItem $stageReferencesFolder -Filter '*.dll' | Sort-Object Name

	[xml] $projectXml = Get-Content $stageProjectPath
	$namespaceUri = $projectXml.Project.NamespaceURI
	$itemGroup = $projectXml.CreateElement('ItemGroup', $namespaceUri)

	foreach ($referenceDll in $referenceDlls) {
		Add-PlgxReference `
			-ProjectXml $projectXml `
			-ItemGroup $itemGroup `
			-AssemblyName ([System.IO.Path]::GetFileNameWithoutExtension($referenceDll.Name)) `
			-HintPath (Join-Path $stageReferencesFolderName $referenceDll.Name)
	}

	[void] $projectXml.Project.InsertBefore($itemGroup, $projectXml.Project.ItemGroup[0])
	$projectXml.Save($stageProjectPath)
}

$pluginVersion = Get-PluginVersion
$plgxOutputPath = Join-Path $root "$pluginName-$pluginVersion.plgx"
$legacyPlgxOutputPath = Join-Path $root "$pluginName.plgx"

Write-Host 'Creating KeePass plugin package'

if (!$SkipBuild) {
	$msBuildPath = Get-MSBuildPath
	$msBuildArgs = @(
		$solutionPath,
		'/restore',
		'/t:Build',
		"/p:Configuration=$configuration",
		'/p:Platform=Any CPU',
		'/p:SkipPlgxPostBuild=true'
	)

	& $msBuildPath @msBuildArgs
	if ($LASTEXITCODE -ne 0) {
		throw "MSBuild failed with exit code $LASTEXITCODE."
	}
}
elseif (!(Test-Path $outputFolder)) {
	throw "Build output folder '$outputFolder' does not exist. Build the project before creating the PLGX package with -SkipBuild."
}

Copy-SourceTree
New-Item -ItemType Directory -Path $stageReferencesFolder | Out-Null

$excludedReferenceNames = @(
	'KeePass.XmlSerializers.dll',
	'KoenZomersKeePassOneDriveSync.dll',
	'System.Net.Http.WebRequest.dll',
	'Test.dll'
)

Get-ChildItem $outputFolder -Filter '*.dll' |
	Where-Object { $excludedReferenceNames -notcontains $_.Name } |
	Copy-Item -Destination $stageReferencesFolder -Force

$netStandardPath = Join-Path ${env:ProgramFiles(x86)} 'Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\Facades\netstandard.dll'
if (Test-Path $netStandardPath) {
	Copy-Item $netStandardPath -Destination $stageReferencesFolder -Force
}
else {
	Write-Warning 'netstandard.dll facade was not found. PLGX compilation may fail for netstandard2.0 dependencies.'
}

Add-PlgxReferencesToProject

if (Test-Path $plgxOutputPath) {
	Remove-Item $plgxOutputPath -Force
}
if (Test-Path $legacyPlgxOutputPath) {
	Remove-Item $legacyPlgxOutputPath -Force
}

$keepassPath = Get-KeePassPath
& $keepassPath --plgx-create $stageSourceFolder --plgx-prereq-net:4.0
if ($LASTEXITCODE -ne 0) {
	throw "KeePass PLGX creation failed with exit code $LASTEXITCODE."
}

$createdPlgxPath = Join-Path $stageRoot "$sourceFolderName.plgx"
for ($attempt = 0; ($attempt -lt 30) -and !(Test-Path $createdPlgxPath); $attempt++) {
	Start-Sleep -Seconds 1
}

if (!(Test-Path $createdPlgxPath)) {
	throw "KeePass did not create the expected package '$createdPlgxPath'."
}

$isPackageUnlocked = $false
for ($attempt = 0; $attempt -lt 30; $attempt++) {
	try {
		$stream = [System.IO.File]::Open($createdPlgxPath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::ReadWrite, [System.IO.FileShare]::None)
		$stream.Dispose()
		$isPackageUnlocked = $true
		break
	}
	catch {
		Start-Sleep -Seconds 1
	}
}

if (!$isPackageUnlocked) {
	throw "KeePass created '$createdPlgxPath', but the file remained locked."
}

Move-Item $createdPlgxPath $plgxOutputPath -Force

if ($CopyToKeePassPluginFolder) {
	$keepassPluginFolder = Join-Path (Split-Path -Parent $keepassPath) 'Plugins'
	if (!(Test-Path $keepassPluginFolder)) {
		New-Item -ItemType Directory -Path $keepassPluginFolder | Out-Null
	}

	$legacyKeePassPluginPath = Join-Path $keepassPluginFolder (Split-Path -Leaf $legacyPlgxOutputPath)
	if (Test-Path $legacyKeePassPluginPath) {
		Remove-Item $legacyKeePassPluginPath -Force
	}

	$keepassPluginPath = Join-Path $keepassPluginFolder (Split-Path -Leaf $plgxOutputPath)
	Copy-Item $plgxOutputPath $keepassPluginPath -Force
	Write-Host "KeePass Plugin package has been copied to: $keepassPluginPath"
}

Write-Host "KeePass Plugin package has been created: $plgxOutputPath"
