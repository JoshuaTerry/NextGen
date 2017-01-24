param (
    [string]$buildConfiguration = "Debug",
    [switch]$deploy = $false,
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

Function RunUnitTests
{
	param (
		[string]$buildConfiguration = "Debug"
	)
 	Write-Host "Running Tests..."
	$TestResultDir = ".\TestResults\"
	Get-ChildItem -Path $TestResultDir -Include * -File -Recurse | foreach { $_.Delete()}
	
	#Find tests in bin that are for the same build configuration
	$tests = (Get-ChildItem "..\" -Recurse -Include *Test*.dll | Where {$_.FullName -notlike "*\obj\*" -And $_.FullName -like "*\$buildConfiguration\*"})
		
	# Run tests
	& vstest.console $tests /TestCaseFilter:TestCategory!="Long Running" /Logger:trx
	
	# Copy Test File to permanent file
	$TrxFile = Get-ChildItem $TestResultDir*.trx
	copy-item $TrxFile -destination .\TestResult.xml
}

Function WereTestsSuccessful
{
	$path = "./TestResult.xml"
	$results = [xml](GC $path)
	$outcome = $results.TestRun.ResultSummary.outcome

	$wasSuccessful = ($outcome -eq 'Completed')
	return $wasSuccessful
}

Function DeployProjects
{
	param (
		[string]$buildConfiguration = "Debug"
	)
 	Write-Host "Deploying API..."
	& $msbuildPath ..\DDI.sln /p:DeployOnBuild=true /p:PublishProfile='DevEnv' /p:Configuration=$buildConfiguration /p:Platform="Any CPU"
}

###############################################################
######               Start Script                     #########
###############################################################
$msbuildPath = "msbuild"
if ($runOnServer)
{
	# Get the path to the MsBuild executable.
	$msBuildPath = Get-MsBuildPath
}
# Restore Nuget packages
& ..\.nuget\NuGet.exe restore ..\DDI.sln

# Calling msbuild will set $LastExitCode to 0 if it was successful. It is a 1 or greater if there was an error
& $msbuildPath ..\DDI.sln /p:Configuration=$buildConfiguration /p:Platform="Any CPU" /t:"Clean;Rebuild"
if ($LastExitCode -eq 0)
{ 
	Write-Host "Build completed successfully." 

	RunUnitTests -buildConfiguration:$buildConfiguration
	$testsAreSuccessful=WereTestsSuccessful
	if ($testsAreSuccessful)
	{
		Write-Host "Tests Completed."
		if ($deploy)
		{
			DeployProjects -buildConfiguration:$buildConfiguration
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
		Write-Host "There are failing tests, so this script is stopping. Please fix the test, or pass in the -ignoreTests parameter."
		ExitWithCode -exitcode 2 
	}

}
else
{ 
	Write-Host "Build failed. Check the build log file for errors."
	ExitWithCode -exitcode 1 
}

Write-Host "All Done!!!"
ExitWithCode -exitcode 0







