param (
	[string]$deployEnvironment = "Dev",
	[string]$workingDirectory = "../"
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

function GetConfigFiles
{
	param
	(
		[string]$workingDirectory = "../"
	)
	return get-childitem $workingDirectory -recurse  | where {$_.extension -eq ".config"}
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
	switch ($deployEnvironment)
	{
		"Test"
		{
			$Config | add-member –membertype NoteProperty –name Environment –value "Test"
			$Config | add-member –membertype NoteProperty –name SmtpHost –value "coloex1.ddi.net"
			$Config | add-member –membertype NoteProperty –name WebRoot –value "localhost:12345"
			$Config | add-member –membertype NoteProperty –name CommonContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Common_TEST;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name DomainContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Demo_TEST;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name WEB_API_ADDRESS –value "http://devapi.ddi.org/api/v1/"
		}
		"Dev"
		{
			$Config | add-member –membertype NoteProperty –name Environment –value "Dev"
			$Config | add-member –membertype NoteProperty –name SmtpHost –value "coloex1.ddi.net"
			$Config | add-member –membertype NoteProperty –name WebRoot –value "localhost:12345"
			$Config | add-member –membertype NoteProperty –name CommonContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Common_DEV;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name DomainContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Demo_DEV;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name WEB_API_ADDRESS –value "http://172.20.10.22:81/api/v1/"
		}
		default  ##Locallhost
		{
			$Config | add-member –membertype NoteProperty –name Environment –value "Local"
			$Config | add-member –membertype NoteProperty –name SmtpHost –value "localhost"
			$Config | add-member –membertype NoteProperty –name WebRoot –value "localhost:12345"
			$Config | add-member –membertype NoteProperty –name CommonContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Common_DEV;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name DomainContext –value "Data Source=tcp:10.200.10.173;Initial Catalog=DDI_Connect_Demo_DEV;User Id=nextgen_web;Password=UGKOIiH4QBJETecrquzP6g==;MultipleActiveResultSets=True"
			$Config | add-member –membertype NoteProperty –name WEB_API_ADDRESS –value "http://localhost:49490/api/v1/"
		}
	}
	return $Config
}

###############################################################
######               Start Script                     #########
###############################################################
$Config = GetConfigValues -deployEnvironment:$deployEnvironment

$Files = GetConfigFiles -workingDirectory:$workingDirectory
foreach($file in $Files)
{
	(Get-Content $file) |  ForEach-Object { $_ -replace "<add key=""Environment"".*/>" , "<add key=""Environment"" value=""$($Config.Environment)"" />" } | Set-Content $file
	(Get-Content $file) |  ForEach-Object { $_ -replace "<add key=""SmtpHost"".*/>" , "<add key=""SmtpHost"" value=""$($Config.SmtpHost)"" />" } | Set-Content $file
	(Get-Content $file) |  ForEach-Object { $_ -replace "<add key=""WebRoot"".*/>" , "<add key=""WebRoot"" value=""$($Config.WebRoot)"" />" } | Set-Content $file
	(Get-Content $file) |  ForEach-Object { $_ -replace "<add name=""CommonContext"".*/>" , "<add name=""CommonContext"" connectionString=""$($Config.CommonContext)"" providerName=""System.Data.SqlClient"" />" } | Set-Content $file
	(Get-Content $file) |  ForEach-Object { $_ -replace "<add name=""DomainContext"".*/>" , "<add name=""DomainContext"" connectionString=""$($Config.DomainContext)"" providerName=""System.Data.SqlClient"" />" } | Set-Content $file

}

$Files = GetJSFiles -workingDirectory:$workingDirectory
foreach($file in $Files)
{
	(Get-Content $file) |  ForEach-Object { $_ -replace "var WEB_API_ADDRESS = '.*';" , "var WEB_API_ADDRESS = '$($Config.WEB_API_ADDRESS)';" } | Set-Content $file

}
Write-Host "All Done!!!"
ExitWithCode -exitcode 0







