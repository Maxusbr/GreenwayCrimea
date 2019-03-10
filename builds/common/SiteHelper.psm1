import-module "WebAdministration" -DisableNameChecking

function CreateSite($site, $path, $poolName)
{	
    $iisAppPoolName = $poolName
    $iisAppPoolDotNetVersion = "v4.0"

    cd IIS:\AppPools\
    #check if the app pool exists
    if (!(Test-Path $iisAppPoolName -pathType container))
    {
        #create the app pool
        $appPool = New-Item $iisAppPoolName
        $appPool | Set-ItemProperty -Name "managedRuntimeVersion" -Value $iisAppPoolDotNetVersion
    }
    cd IIS:\Sites\DefaultWebSite
    if (Test-Path $site -pathType container)
    {
        return
    }
    #$iisApp = New-Item $site -bindings @{protocol="http";bindingInformation=":80:" + $site} -physicalPath $path
    #$iisApp | Set-ItemProperty -Name "applicationPool" -Value $iisAppPoolName	
    $iisApp = New-Item IIS:\Sites\DefaultWebSite\$site -physicalPath $path -type Application
    Set-ItemProperty IIS:\sites\DefaultWebSite\$site -name applicationPool -value $iisAppPoolName
}

function DoRequest($url)
{	
	$webclient = New-Object Net.WebClient	
	$stringSite = $webclient.DownloadString($url);
    if (-not $stringSite.Contains('<meta name="generator" content="AdVantShop.NET">'))
    {
        $errMsg = "Error on do request \n\r" + $stringSite
        throw [Exception] $errMsg
    }
}

function DoRequestRetry($url)
{
	$retryCount =3
	For ($i=0; $i -lt $retryCount; $i++) {    
		try 
		{
			DoRequest($url)
			return
		}
		catch [Exception]
		{			
		}
	}
	throw $_.Exception
}

function SetConnectionString($sitepath, $servername, $database, $username, $password)
{
    $config = $sitepath + '\Web.ConnectionString.config'
	
	If(!(Test-Path $config)){
		$configEtalon = $sitepath + '\Web.ConnectionString.config.etalon'
		renameFile $configEtalon $config
	}
	
    $doc = (Get-Content $config) -as [Xml]
    $root = $doc.get_DocumentElement();
    #replace clean
    $newCon = $root.add.connectionString.Replace("Data Source='MyServerName'; Connect Timeout='60'; Initial Catalog='MyDBName'; Persist Security Info='True'; User ID='MyUserName'; Password='MyPass';", "Data Source='$servername'; Connect Timeout='60'; Initial Catalog='$database'; Persist Security Info='True'; User ID='$username'; Password='$password';");
    $root.add.connectionString = $newCon
    #replace dev
    $newCon = $root.add.connectionString.Replace("Data Source='SERVER\SQL2008R2EXPRESS'; Connect Timeout='60'; Initial Catalog='AdvantShop50_etalon'; Persist Security Info='True'; User ID='sa'; Password='ewqEWQ321#@!';", "Data Source='$servername'; Connect Timeout='60'; Initial Catalog='$database'; Persist Security Info='True'; User ID='$username'; Password='$password';");
    $root.add.connectionString = $newCon
    $doc.Save($config)
}

function SetDefaultConnectionString($sitepath)
{
    $config = $sitepath + '\Web.ConnectionString.config'
    $doc = (Get-Content $config) -as [Xml]
    $root = $doc.get_DocumentElement();
    $newCon = "Data Source='MyServerName'; Connect Timeout='60'; Initial Catalog='MyDBName'; Persist Security Info='True'; User ID='MyUserName'; Password='MyPass';"
    $root.add.connectionString = $newCon
    $doc.Save($config)
}

function SetMode($sitepath, $mode)
{
    $config = $sitepath + '\Web.ModeSettings.config'
    $doc = (Get-Content $config) -as [Xml]
	if ($mode.equals('Saas')){
		$node = $doc.modesettings.SaasMode;
		$node.value = "True";        
	}
	if ($mode.equals('Trial')){
		$node = $doc.modesettings.TrialMode;
		$node.value = "True";        
	}

    $doc.Save($config)
}

Export-ModuleMember -Function  *