﻿param (
    [string]$buildConfiguration = "Debug",
    [string]$deployEnvironment = "Dev",
	[string]$workingDirectory = "C:\Users\jterry\Documents\git\nextgen\DDI",
    [switch]$deploy = $true,
    [switch]$ignoreTests = $false,
    [switch]$runOnServer = $false
)
  
function ExitWithCode 
{ 
    param 
    ( 
        $exitcode 
    )
    #$host.SetShouldExit($exitcode) 
    exit $exitcode
}

function Get-MsBuildPath
{
<#
	.SYNOPSIS
	Gets the path to the latest version of MsBuild.exe. Throws an exception if MSBuild.exe is not found.
	
	.DESCRIPTION
	Gets the path to the latest version of MsBuild.exe. Throws an exception if MSBuild.exe is not found.
#>

	# Get the path to the directory that the latest version of MSBuild is in.
	$MsBuildToolsVersionsStrings = Get-ChildItem -Path 'HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\' | Where-Object { $_ -match '[0-9]+\.[0-9]' } | Select-Object -ExpandProperty PsChildName
	[double[]]$MsBuildToolsVersions = $MsBuildToolsVersionsStrings | ForEach-Object { [Convert]::ToDouble($_) }
	$LargestMsBuildToolsVersion = $MsBuildToolsVersions | Sort-Object -Descending | Select-Object -First 1 
	$MsBuildToolsVersionsKeyToUse = Get-Item -Path ('HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\{0:n1}' -f $LargestMsBuildToolsVersion)
	$MsBuildDirectoryPath = $MsBuildToolsVersionsKeyToUse | Get-ItemProperty -Name 'MSBuildToolsPath' | Select -ExpandProperty 'MSBuildToolsPath'

	if(!$MsBuildDirectoryPath)
	{
		throw 'MsBuild.exe was not found on the system.'          
	}

	# Get the path to the MSBuild executable.
	$MsBuildPath = (Join-Path -Path $MsBuildDirectoryPath -ChildPath 'msbuild.exe')

	if(!(Test-Path $MsBuildPath -PathType Leaf))
	{
		throw 'MsBuild.exe was not found on the system.'          
	}

	return $MsBuildPath
}

Function DeployProjects
{
	param (
		[string]$buildConfiguration = "Debug",
		[string]$deployEnvironment = "Dev"
	)
	& ./Rake.ps1 -deployEnvironment:$deployEnvironment
 	Write-Host "Deploying API... $deployEnvironment"
	& $msbuildPath ..\DDI.sln /p:DeployOnBuild=true /p:PublishProfile=$($deployEnvironment)Env /p:Configuration=$buildConfiguration /p:Platform="Any CPU"
}





function ExitWithCode 
{ 
    param 
    ( 
        $exitcode 
    )
    #$host.SetShouldExit($exitcode) 
    exit $exitcode
}

function GetConfigFiles
{
	param
	(
		[string]$workingDirectory = "../"
	)
	return get-childitem $workingDirectory -recurse  | where {$_.name -eq "web.config"}
}

function GetJSFiles
{
	param
	(
		[string]$workingDirectory = "../"
	)
	return get-childitem $workingDirectory -recurse  | where {$_.name -eq "configuration.js"}
}

function GetConfigValues
{
	param
	(
		[string]$deployEnvironment="Dev"
	)
	$Config = new-object psobject
	 
	$Config | add-member –membertype NoteProperty –name Environment –value "Dev"
			$Config | add-member –membertype NoteProperty –name SmtpHost –value "coloex1.ddi.net"
			$Config | add-member –membertype NoteProperty –name WebRoot –value "localhost:12345"
			$Config | add-member –membertype NoteProperty –name ElasticsearchURL –value "http://10.200.10.173:9200"
			$Config | add-member –membertype NoteProperty –name CommonContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Common_DEV;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name DomainContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Demo_DEV;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name WEB_API_ADDRESS –value "http://172.20.10.22:81/api/v1/"
		 
	return $Config
}

###############################################################
######               Start Script                     #########
###############################################################
$Config = GetConfigValues -deployEnvironment:$deployEnvironment

$Files = GetConfigFiles -workingDirectory:$workingDirectory
foreach($file in $Files)
{
	(Get-Content $file.FullName) |  ForEach-Object { $_ -replace "<add key=""Environment"".*/>" , "<add key=""Environment"" value=""$($Config.Environment)"" />" } | Set-Content $file.FullName
	(Get-Content $file.FullName) |  ForEach-Object { $_ -replace "<add key=""SmtpHost"".*/>" , "<add key=""SmtpHost"" value=""$($Config.SmtpHost)"" />" } | Set-Content $file.FullName
	(Get-Content $file.FullName) |  ForEach-Object { $_ -replace "<add key=""WebRoot"".*/>" , "<add key=""WebRoot"" value=""$($Config.WebRoot)"" />" } | Set-Content $file.FullName
	(Get-Content $file.FullName) |  ForEach-Object { $_ -replace "<add key=""ElasticsearchURL"".*/>" , "<add key=""ElasticsearchURL"" value=""$($Config.ElasticsearchURL)"" />" } | Set-Content $file.FullName

	(Get-Content $file.FullName) |  ForEach-Object { $_ -replace "<add name=""CommonContext"".*/>" , "<add name=""CommonContext"" connectionString=""$($Config.CommonContext)"" providerName=""System.Data.SqlClient"" />" } | Set-Content $file.FullName
	(Get-Content $file.FullName) |  ForEach-Object { $_ -replace "<add name=""DomainContext"".*/>" , "<add name=""DomainContext"" connectionString=""$($Config.DomainContext)"" providerName=""System.Data.SqlClient"" />" } | Set-Content $file.FullName

}

$Files = GetJSFiles -workingDirectory:$workingDirectory
foreach($file in $Files)
{
	(Get-Content $file.FullName) |  ForEach-Object { $_ -replace "var WEB_API_ADDRESS = '.*';" , "var WEB_API_ADDRESS = '$($Config.WEB_API_ADDRESS)';" } | Set-Content $file.FullName

}
 
 
	# $msBuildPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
$msBuildPath = Get-MsBuildPath
 
# Restore Nuget packages
& "C:\Users\jterry\Documents\git\nextgen\DDI\.nuget\NuGet.exe" restore C:\Users\jterry\Documents\git\nextgen\DDI\DDI.sln

# Calling msbuild will set $LastExitCode to 0 if it was successful. It is a 1 or greater if there was an error
& $msbuildPath C:\Users\jterry\Documents\git\nextgen\DDI\DDI.sln /p:Configuration=$buildConfiguration /p:Platform="Any CPU" /t:"Clean;Rebuild"
if ($LastExitCode -eq 0)
{ 
	Write-Host "Build completed successfully." 
 
		if ($deploy)
		{
			DeployProjects -buildConfiguration:$buildConfiguration -deployEnvironment:$deployEnvironment
			if ($LastExitCode -eq 0)
			{ 
				Write-Host "Website and APIs have been published"		
			}
			else
			{
				Write-Host "There was an error Publishing the projects."
				ExitWithCode -exitcode 3						
			}
		}
		 
}
else
{ 
	Write-Host "Build failed. Check the build log file for errors."
	ExitWithCode -exitcode 1 
}

Write-Host "All Done!!!"
ExitWithCode -exitcode 0
