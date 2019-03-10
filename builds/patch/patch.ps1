param (
        [String]$sqlName = "ADV-SAND-SRV\SQLEXPRESS",        
        [String]$chekoutpath ="E:\teamcity\buildAgent\work\8691eb17a5f15243",
        [String]$pathtoDb ="E:\db",                
        [String]$sitepath = "E:\sites\patch\current",
        [String]$poolName ="dev",
        [String]$outputpath ="E:\advantshopOut\patch"
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

$database = "patch" + $version
$site ="patch" + $version

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
	Write-Host "patch"
	$outPath = $outputpath + "\"+$site
	
	deleteFolder $outPath
    createFolder $outPath
		
	$sqlpath = $outPath +"\_SQL"

	createFolder $sqlpath

	foreach ($f in Get-ChildItem -path $scriptpath -Filter *.sql | sort-object { [regex]::Replace($_.Name, '\d+', { $args[0].Value.PadLeft(20) }) } ) 
	{ 
		if($f.FullName.Contains($version)){
			Copy-Item $f.FullName $sqlpath
		}
	}	
	    	
	$patchPath = $outPath +"\patch"
	$exclude = @('AdvantShop.sln.metaproj','AdvantShop.sln.metaproj.tmp', 'install.txt', 'Web.ModeSettings.config', 'Web.ConnectionString.config', 'Advantshop.Web.Site.csproj.teamcity','Advantshop.Web.Site.csproj.teamcity.msbuild.tcargs','gulpfile.js','job_scheduling_data_2_0.xsd','karma.conf.js','robots.txt' )
	$excludeMatch = @('\pictures\','\userfiles\' , '\combine\' , '^\modules\', '\pictures\' , '\userfiles\' , '\export\',  '\errlog\')
	copyFolderRetry $sitepath $patchPath $exclude $excludeMatch	   

	deleteCommonFiles($patchPath)	
	
    Write-Host "zip site"
    #zip site
    $zipSite = $outPath + "\" + $site + ".zip"
    $zipFolderPath = $outPath + "\*"
	zip $zipSite $zipFolderPath			

	deleteFolderRetry $patchPath
	deleteFolderRetry $sqlpath
	
	Write-Host "done"
}
catch [Exception]
{
	#Write-Host $_.Exception.Message
	$errorsReported = $False
	CheckForErrors $Error
	exit 1
}