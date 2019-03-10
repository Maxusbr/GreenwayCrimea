param (
        [String]$sqlName = "ADV-SAND-SRV\SQLEXPRESS",        
        [String]$chekoutpath ="E:\teamcity\buildAgent\work\8691eb17a5f15243",
        [String]$pathtoDb ="E:\db",                
        [String]$sitepath = "E:\sites\saas\current",
        [String]$poolName ="dev",
        [String]$outputpath ="E:\advantshopOut\saas",
		[String]$username= "sa",
		[String]$password= "qweasdefgw123!"
)

$ScriptDir = Split-Path -parent $MyInvocation.MyCommand.Path
Import-Module $ScriptDir\..\Common\SqlHelper.psm1
Import-Module $ScriptDir\..\Common\SiteHelper.psm1
Import-Module $ScriptDir\..\Common\Helper.psm1
$ErrorActionPreference = "Stop"

if (!(Test-Path -Path $sitepath))
{
    exit 0;
}

$backupPath = $chekoutpath + "\DataBase\AdvantShop_6.0.8.bak"
$scriptpath = $chekoutpath + "\DataBase\patches"

$version = getVersion $sitepath

$database = "saas" + $version
$site ="saas" + $version

renameFolder $sitepath $site

$sitepath=$sitepath.Replace('current',$site)

Write-Host "sqlName" $sqlName
Write-Host "database" $database
Write-Host "backupPath" $backupPath
Write-Host "pathtoDb" $pathtoDb
Write-Host "scriptpath" $scriptpath
Write-Host "site" $site
Write-Host "sitepath" $sitepath
Write-Host "poolName" $poolName
Write-Host "outputpath" $outputpath

try 
{
	Write-Host "Saas"
	Write-Host "restore database"
	RestoreDb $sqlName $database $pathtoDb $backupPath
	
	foreach ($f in Get-ChildItem -path $scriptpath -Filter *.sql | sort-object { [regex]::Replace($_.Name, '\d+', { $args[0].Value.PadLeft(20) }) } ) 
	{ 
	    Write-Host "aplly file " $f.fullname
		SqlStringExec $sqlName $database $f.fullname
	}
	SqlCommandExec $sqlName $database "update [Catalog].[Photo] set PhotoName = 'http://cs71.advantshop.net/'+ PhotoName"
	
    $fromPath =$chekoutpath + "\AdvantShop.Web\pictures";
	$toPath= $sitepath  + "\pictures";

	$exclude = @('')
	$excludeMatch = @('\pictures\product\', '\pictures\category\')
	
    copyFolderRetry $fromPath $toPath $exclude $excludeMatch

    $fromPath =$chekoutpath + "\AdvantShop.Web\userfiles";
	$toPath= $sitepath  + "\userfiles"; 
    copyFolderRetry  $fromPath $toPath

    $fromPath =$chekoutpath + "\AdvantShop.Web\design";
	$toPath= $sitepath  + "\design"; 
    copyFolderRetry $fromPath $toPath

	SetMode $sitepath "Saas"

	Write-Host "set connection " $site
	SetConnectionString $sitepath $sqlName $database $username $password
	
	Write-Host "create site " $site
	CreateSite $site $sitepath $poolName
	
	Write-Host "do request to site " $site
	$url = "http://localhost/" + $site
	DoRequestRetry $url

    $outPath = $outputpath + "\"+$site
    $backupfile = $outPath + "\" + $site + ".bak"
    $zipdb = $outPath + "\" + $site + "_db.zip"

    deleteFolder $outPath
    createFolder $outPath
    	
    Write-Host "backupDb"
	backupDb $sqlName $database $backupfile
	
    Write-Host "zip db"
    #zip db
	zip $zipdb $backupfile  
	
    SetDefaultConnectionString $sitepath

	deleteCommonFiles($sitepath)	
	
    Write-Host "zip site"
    #zip site
    $zipSite = $outPath + "\" + $site + "_code.zip"
    $zipFolderPath = $sitepath + "\*"
	zip $zipSite $zipFolderPath
	
    SetConnectionString $sitepath $sqlName $database $username $password
	#uploadtocap

    deleteFile $backupfile
	
	Write-Host "done"
}
catch [Exception]
{
	#Write-Host $_.Exception.Message
	$errorsReported = $False
	CheckForErrors $Error
	exit 1
}