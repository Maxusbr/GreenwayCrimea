param (
        [String]$sqlName = "ADV-SAND-SRV\SQLEXPRESS",        
        [String]$chekoutpath ="E:\teamcity\buildAgent\work\8691eb17a5f15243",
        [String]$pathtoDb ="E:\db",                
        [String]$sitepath = "E:\sites\trial\current",
        [String]$poolName ="dev",
        [String]$outputpath ="E:\advantshopOut\trial",
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

$database = "trial" + $version
$site ="trial" + $version

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
	Write-Host "Trial"
	Write-Host "restore database"
	RestoreDb $sqlName $database $pathtoDb $backupPath
	
	foreach ($f in Get-ChildItem -path $scriptpath -Filter *.sql | sort-object { [regex]::Replace($_.Name, '\d+', { $args[0].Value.PadLeft(20) }) } ) 
	{ 
	    Write-Host "aplly file " $f.fullname
		SqlStringExec $sqlName $database $f.fullname
	}
	
	SqlCommandExec $sqlName $database "update [Catalog].[Photo] set PhotoName = 'http://cs71.advantshop.net/'+ PhotoName"
	SqlCommandExec $sqlName $database "INSERT INTO [dbo].[DownloadableContent] ([StringId], [IsInstall], [DateAdded], [DateModified], [Active], [Version],[DcType])  VALUES ('Ermitage', 1, getdate(), getdate(), 1, 1, 'template')"
	SqlCommandExec $sqlName $database "INSERT INTO [dbo].[DownloadableContent] ([StringId], [IsInstall], [DateAdded], [DateModified], [Active], [Version],[DcType])  VALUES ('Moloko', 1, getdate(), getdate(), 1, 1, 'template')"
	SqlCommandExec $sqlName $database "INSERT INTO [dbo].[Modules] ([ModuleStringID], [IsInstall], [DateAdded], [DateModified], [Active], [Version],[NeedUpdate])  VALUES ('VkMarket', 1, getdate(), getdate(), 1, 1, 0)"
	
	SqlCommandExec $sqlName $database "Update [Order].[OrderCustomer] set house = '6', structure = '2', apartment = '439', entrance = '3', floor = '8', email='alex-test@sidorov.ru', phone='+7900123456', standardphone='7900123456'  where OrderID = 1"
	SqlCommandExec $sqlName $database "INSERT INTO [Settings].[Settings] ([Name],[Value]) VALUES ('CustomersNotifications.ShowCookiesPolicyMessage', 'True')"
	SqlCommandExec $sqlName $database "update [Catalog].[Product] set IsDemo = 1"
	SqlCommandExec $sqlName $database "update [Catalog].[Category] set IsDemo = 1 WHERE CategoryID <> 0"
	SqlCommandExec $sqlName $database "update [Catalog].[Brand] set IsDemo = 1"
	SqlCommandExec $sqlName $database "if not exists (Select SettingId From [Settings].[Settings] Where Name = 'IsDefaultLogo') 
	Insert Into [Settings].[Settings] ([Name],[Value]) Values ('IsDefaultLogo', 'True')"
	SqlCommandExec $sqlName $database "if not exists (Select SettingId From [Settings].[Settings] Where Name = 'IsDefaultSlides') 
	Insert Into [Settings].[Settings] ([Name],[Value]) Values ('IsDefaultSlides', 'True')"

	
	$fromPath =$chekoutpath + "\AdvantShop.Web\pictures";
	$toPath= $sitepath  + "\pictures"; 
	
	$exclude = @('some_file_wich_doesnot_exsists.txt')
	$excludeMatch = @('\product\', '\category\')
	
    copyFolderRetry $fromPath $toPath $exclude $excludeMatch

    $fromPath =$chekoutpath + "\AdvantShop.Web\userfiles";
	$toPath= $sitepath  + "\userfiles"; 
    copyFolderRetry  $fromPath $toPath

    $fromPath =$chekoutpath + "\AdvantShop.Web\design";
	$toPath= $sitepath  + "\design"; 
    copyFolderRetry $fromPath $toPath

	SetMode $sitepath "Trial"
	
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