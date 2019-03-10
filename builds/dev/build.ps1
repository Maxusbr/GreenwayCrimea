param (
        [String]$sqlName = "ADV-SAND-SRV\SQLEXPRESS",        
        [String]$chekoutpath ="E:\teamcity\buildAgent\work\8691eb17a5f15243",
        [String]$pathtoDb ="E:\db",                
        [String]$sitepath = "E:\sites\dev",
		[String]$sitepathsaas = "E:\sites\devsaas",
		[String]$testsitepath = "E:\sites\devtest",
        [String]$poolName ="dev",
		[String]$siteType = "lic",
		[String]$username= "sa",
		[String]$password= "qweasdefgw123!"
)

$ScriptDir = Split-Path -parent $MyInvocation.MyCommand.Path
Import-Module $ScriptDir\..\Common\SqlHelper.psm1
Import-Module $ScriptDir\..\Common\SiteHelper.psm1
Import-Module $ScriptDir\..\Common\Helper.psm1
$ErrorActionPreference = "Stop"

$backupPath = $chekoutpath + "\DataBase\AdvantShop_6.0.8.bak"
$scriptpath = $chekoutpath + "\DataBase\patches"

$database = "dev"
$databasesaas = "devsaas"
$databasetest = "devtest"
$site = "dev"
$sitesaas = "devsaas"
$sitetest = "devtest"

Write-Host "sqlName" $sqlName
Write-Host "database" $database
Write-Host "backupPath" $backupPath
Write-Host "pathtoDb" $pathtoDb
Write-Host "scriptpath" $scriptpath
Write-Host "site" $site
Write-Host "sitepath" $sitepath
Write-Host "sitesaaspath" $sitesaaspath
Write-Host "testsitepath" $testsitepath
Write-Host "poolName" $poolName

try 
{
	Write-Host "restore database"
	RestoreDb $sqlName $database $pathtoDb $backupPath
	RestoreDb $sqlName $sitetest $pathtoDb $backupPath
	
	if($siteType -eq "saas"){
		RestoreDb $sqlName $sitesaas $pathtoDb $backupPath
	}
	
	foreach ($f in Get-ChildItem -path $scriptpath -Filter *.sql | sort-object { [regex]::Replace($_.Name, '\d+', { $args[0].Value.PadLeft(20) }) } ) 
	{ 
	    Write-Host "aplly file " $f.fullname $database
		SqlStringExec $sqlName $database $f.fullname
		
		Write-Host "aplly file " $f.fullname $databasetest
		SqlStringExec $sqlName $databasetest $f.fullname
		
		if($siteType -eq "saas"){
			Write-Host "aplly file " $f.fullname $databasesaas
			SqlStringExec $sqlName $databasesaas $f.fullname
		}
	}
	
	SqlCommandExec $sqlName $database "UPDATE Settings.Settings set value = '8b40c4f4-322e-4926-ad39-d2f6d6cd10c1' where name='lickey'; UPDATE Settings.Settings set value = 'True' where name='ActiveLic'"
	SqlCommandExec $sqlName $databasetest "UPDATE Settings.Settings set value = '8b40c4f4-322e-4926-ad39-d2f6d6cd10c1' where name='lickey'; UPDATE Settings.Settings set value = 'True' where name='ActiveLic'"
		
	if($siteType -eq "saas"){
		SqlCommandExec $sqlName $databasesaas "UPDATE Settings.Settings set value = 'e6dc57ee-931e-4ac1-9137-5a47e56f977a' where name='lickey'; ; UPDATE Settings.Settings set value = 'True' where name='ActiveLic'"
	}
		
	
	SqlCommandExec $sqlName $database "insert Settings.Settings (name, value) values ('ClientCode','240')"
	SqlCommandExec $sqlName $databasetest "insert Settings.Settings (name, value) values ('ClientCode','240')"
	
	if($siteType -eq "saas"){
		SqlCommandExec $sqlName $databasesaas "insert Settings.Settings (name, value) values ('ClientCode','76001')"
	}
	
	$fromPath =$chekoutpath + "\AdvantShop.Web\pictures";
	$toPath= $sitepath  + "\pictures"; 
    	copyFolderRetry $fromPath $toPath 

    $fromPath =$chekoutpath + "\AdvantShop.Web\userfiles";
	$toPath= $sitepath  + "\userfiles"; 
    copyFolderRetry  $fromPath $toPath

    $fromPath =$chekoutpath + "\AdvantShop.Web\design";
	$toPath= $sitepath  + "\design"; 
    copyFolderRetry $fromPath $toPath
	
	if($siteType -eq "saas"){
		# creating saas copy
		Write-Host "delete saas site " $sitepathsaas
		Get-ChildItem -Path $sitepathsaas -Include * -File -Recurse | foreach { $_.Delete()} 
	
	
		Write-Host "crete saas site folder" $sitepathsaas
		createFolder $sitepathsaas
	
		Write-Host "copy saas site " $sitepathsaas
		copyFolderRetry $sitepath $sitepathsaas
	}
	
	# creating test copy
	Write-Host "delete test site " $testsitepath
	Get-ChildItem -Path $testsitepath -Include * -File -Recurse | foreach { $_.Delete()} 
	
	Write-Host "crete test site folder" $testsitepath
	createFolder $testsitepath
	
	Write-Host "copy test site " $testsitepath
	copyFolderRetry $sitepath $testsitepath

	
	# set all connections
	Write-Host "set connection " $site
	SetConnectionString $sitepath $sqlName $database $username $password
	
	if($siteType -eq "saas"){
		Write-Host "set connection " $sitesaas
		SetConnectionString $sitepathsaas $sqlName $databasesaas $username $password
		SetMode $sitepathsaas "Saas"
	}
	
	Write-Host "set connection " $sitetest
	SetConnectionString $testsitepath $sqlName $databasetest $username $password
	
	Write-Host "create site " $site
	CreateSite $site $sitepath $poolName
	
	if($siteType -eq "saas"){
		Write-Host "create saas site " $sitesaas
		CreateSite $sitesaas $sitepathsaas $poolName
	}
	
	Write-Host "create test site " $sitetest
	CreateSite $sitetest $testsitepath $poolName
	
	# pinging
	Write-Host "do request to site " $site
	$url = "http://localhost/" + $site
	DoRequestRetry $url

	if($siteType -eq "saas"){
		Write-Host "do request to saas site " $sitesaas
		$url = "http://localhost/" + $sitesaas
		DoRequestRetry $url
	}
	
	Write-Host "do request to test site " $sitetest
	$url = "http://localhost/" + $sitetest
	DoRequestRetry $url
	
	
	Write-Host "done"
}
catch [Exception]
{
	#Write-Host $_.Exception.Message
	$errorsReported = $False
	CheckForErrors $Error
	exit 1
}