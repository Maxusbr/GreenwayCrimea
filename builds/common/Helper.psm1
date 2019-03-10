
function CheckForErrors ($error)
{   
   if($error.Count -ne 0)
   {
           write-host "******************************"
           write-host "Errors:", $error.Count 
           write-host "******************************"
           foreach($err in $error)
           {
                   $errorsReported  = $True
                   if( $err.Exception.InnerException -ne $null)
                   {
                           write-host $err.Exception.InnerException.ToString()
                   }
                   else
                   {
                           write-host $err.Exception.ToString()
                   }
                           
                   write-host "----------------------------------------------"
           }           
           
   }
}

function zip($destination, $source)
{
    if (-not (test-path "$env:ProgramFiles\7-Zip\7z.exe")) {throw "$env:ProgramFiles\7-Zip\7z.exe needed"} 
    set-alias sz "$env:ProgramFiles\7-Zip\7z.exe"  

    sz a -tzip $destination $source
}

function ZipFolder( $zipfilename, $sourcedir )
{
     Add-Type -Assembly System.IO.Compression.FileSystem 
     $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
     [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir, $zipfilename, $compressionLevel, $false) 
}

function ZipSingle ($source, $destination)
{
   [System.Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem") | Out-Null
   $zipEntry = "$source" | Split-Path -Leaf
   $zipFile = [System.IO.Compression.ZipFile]::Open($destination, 'Update')
   $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
   [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zipfile,$Source,$zipEntry,$compressionLevel)
    Write-Verbose "Created archive $destination."
}

function UploadFile($sourceFilePath)
{
    $siteAddress = "http://cap.advantshop.net"
    $urlDest = "{0}/{1}" -f ($siteAddress, "BlankSource/test")
    $webClient = New-Object System.Net.WebClient
	#$webClient.Headers.Add("Content-Type", "image/jpeg");
    #$webClient.Credentials = New-Object System.Net.NetworkCredential("MyUserName", "MyPassword")
    ("*** Uploading {0} file to {1} ***" -f ($sourceFilePath, $siteAddress) ) | write-host -ForegroundColor Green -BackgroundColor Yellow
    $webClient.UploadFile($urlDest, "POST", $sourceFilePath)
	$webClient.Dispose()
}

function UploadFile2()
{
	$user = "admin"
	$pass = "admin123"
	$dir = "C:\Virtual Hard Disks"
	$fileName = "win2012r2std.vhdx"
	$file = "$dir/$fileName"
	$url = "http://nexus.lab.local:8081/nexus/content/sites/myproj/$fileName"
	$Timeout=10000000
	$bufSize=10000
	
	$cred = New-Object System.Net.NetworkCredential($user, $pass)
	
	$webRequest = [System.Net.HttpWebRequest]::Create($url)
	$webRequest.Timeout = $timeout
	$webRequest.Method = "POST"
	$webRequest.ContentType = "application/data"
	$webRequest.AllowWriteStreamBuffering=$false
	$webRequest.SendChunked=$true # needed by previous line
	$webRequest.Credentials = $cred
	
	$requestStream = $webRequest.GetRequestStream()
	$fileStream = [System.IO.File]::OpenRead($file)
	$chunk = New-Object byte[] $bufSize
	  while( $bytesRead = $fileStream.Read($chunk,0,$bufsize) )
	  {
	    $requestStream.write($chunk, 0, $bytesRead)
	    $requestStream.Flush()
	  }
	
	$responceStream = $webRequest.getresponse()
	#$status = $webRequest.statuscode
	
	$FileStream.Close()
	$requestStream.Close()
	$responceStream.Close()
	
	$responceStream
	$responceStream.GetResponseHeader("Content-Length") 
	$responceStream.StatusCode
	#$status
}

function createFolder($outPath)
{
    If (!(Test-Path $outPath)) { 
        New-Item -Path $outPath -ItemType Directory
    }
}

function deleteFolder($outPath)
{
    If ((Test-Path $outPath)) {
		get-childitem -Path $outPath -Recurse | Remove-Item -Force -Recurse -Confirm:$false
        Remove-Item $outPath -Force -Recurse
     }
    #Get-ChildItem -Path  $outPath -Recurse
    #-exclude somefile.txt |
    #Select -ExpandProperty FullName |
    #Where {$_ -notlike 'C:\temp\foldertokeep*'} |
    #sort length -Descending |
    #Remove-Item -force
}

function deleteFolderRetry()
{
    param (
            [string] $outPath            
            )

    $retryCount =5
	For ($i=0; $i -lt $retryCount; $i++) {    
		try 
		{
			deleteFolder $outPath 
			return
		}
		catch [Exception]
		{	
            Write-Host "delete err, trying again"
            #CheckForErrors $Error		
		}
	}
	throw $_.Exception
}

function deleteFile($outPath)
{
	Write-Host "delete " $outPath
    If ((Test-Path $outPath)) {
        Remove-Item $outPath 
     }    
}

function deleteFiles($outPath, $extension)
{
	#not working
	#gci | Where-Object {".mp3",".mp4" -eq $_.extension}
	Get-ChildItem $outPath -recurse | Where-Object {$_.Extension -eq $extension} | Remove-Item -Force -Recurse -Confirm:$false
}

function deleteCommonFiles($patchPath)
{
	Write-Host "delete *.scss"
	#deleteFiles($patchPath, ".scss") 
	Get-ChildItem $patchPath -recurse -include *.scss | Remove-Item -Force -Recurse -Confirm:$false
	
	deleteFile($patchPath +"\Web.ConnectionString.config.etalon")
	deleteFile($patchPath +"\package.json.etalon")
	deleteFile($patchPath +"\karma.conf.js ")
	deleteFile($patchPath +"\job_scheduling_data_2_0.xsd ")
	deleteFile($patchPath +"\gulpfile.js")
	deleteFile($patchPath +"\AdvantShop.Web.Site.csproj.teamcity.msbuild.tcargs ")
	deleteFile($patchPath +"\AdvantShop.Web.Site.csproj.teamcity ")
}

function renameFolder($from , $to)
{
    $temp=$from.Replace('current',$to)
	deleteFolderRetry($temp)
	Rename-Item $from $to
}

function renameFile($from, $to)
{
	Write-Host "rename from " $from " to " $to
	If ((Test-Path $from)) {
		Rename-Item $from $to
	}
}


function copyFolder (){

    param (
            [string] $from,
            [string] $to,
            $exclude = @("ttt.ttx"),
			$excludeMatch = @("qwqwqwq")
            )
    Write-Host "copy " $from " to " $to
    [regex] $excludeMatchRegEx = '(?i)' + (($excludeMatch |foreach {[regex]::escape($_)}) –join "|") + ''
    Get-ChildItem -Path $from -Recurse -Exclude $exclude | 
            where { $excludeMatch -eq $null -or $_.FullName.Replace($from, "") -notmatch $excludeMatchRegEx} |
            Copy-Item -Destination {
            $len = $from.length
            if ($_.PSIsContainer) {
                Join-Path $to $_.Parent.FullName.Substring($len)
            } else {
                Join-Path $to $_.FullName.Substring($len)
            }
    } -Force -Exclude $exclude 
 }

function copyFolderRetry()
{
    param (
            [string] $from,
            [string] $to,
            $exclude = @("ttt.ttx"),
			$excludeMatch = @("qwqwqwq")
            )

    $retryCount =5
	For ($i=0; $i -lt $retryCount; $i++) {    
		try 
		{
			copyFolder $from $to $exclude $excludeMatch
			return
		}
		catch [Exception]
		{	
            Write-Host "copy err, trying again"
            #CheckForErrors $Error		
		}
	}
	throw $_.Exception
}

function getVersion($sitepath)
{
	Write-Host "read config" ($sitepath + "\web.config")
	$configurationAppSettingXmlPath = "//configuration/appSettings"
	[xml]$configurationDocument = Get-Content -Path ($sitepath + "\web.config")
	$appSettingsNode = $configurationDocument.SelectSingleNode($configurationAppSettingXmlPath)
	$nodeVersion = $configurationDocument.SelectSingleNode($configurationAppSettingXmlPath+"/add[@key='DB_Version']")

	$version = $nodeVersion.value
	return $version
}

function usingClose
{
    param (
        [System.IDisposable] $inputObject = $(throw "The parameter -inputObject is required."),
        [ScriptBlock] $scriptBlock = $(throw "The parameter -scriptBlock is required.")
    )
    
    Try {
        &$scriptBlock
    } Finally {
        if ($inputObject -ne $null) {
            if ($inputObject.psbase -eq $null) {
                $inputObject.Dispose()
            } else {
                $inputObject.psbase.Dispose()
            }
        }
    }
}

function createCodeMaskFile($sourcePath, $codeMasksFile)
{
    #$lengthCopy = $sourcePath.Length
    #$lastIndexOf = $sourcePath.LastIndexOf("\")
    #$lengthCopy=$lengthCopy- $lastIndexOf

	#$fileCodeMask = $codeMasksDirectory + $sourcePath.Substring($lastIndexOf, $lengthCopy ) + ".txt";	
	$fileCodeMask = $codeMasksFile
	$allFiles = [System.IO.Directory]::GetFiles($sourcePath, "*.*",  [System.IO.SearchOption]::AllDirectories);
	$ExclusionFoldersAndFiles = 
                @(
                   ".svn\",
                    "exports\",
                    "Export\\",
                    "pictures\\",
                    "pictures_elbuz\\",
                    "pictures_default\\",
                    "pictures_deleted\\",
                    "pictures_extra\\",
                    "price_download\\",
                    "price_temp\\",
                    "upload_images\\",
                    "combine\\",
                    "userfiles\\",
                    "App_Data\\Lucene\\",
                    "App_Data\\errlog\\",
                    "App_Data\\notepad\\",
                    "App_Data\\publishprofiles",                    
                    "_rev\\",
                    "_SQL\\",
                    "design\\",
                    "info\\",
                    "Documentation\\",

                    "App_Data\\shopBaseMaskFile.txt",
                    "App_Data\\shopCodeMaskFile.txt",
                    "App_Data\\bak.sql",
                    "App_Data\\dak_code.zip",
                    "App_Data\\LogTempData.txt",
                    "App_Data\\template.config",
                    "scripts\\_localization\\",
                    "css\\extra.css",

                    "Web.ConnectionString.config",
                    "Web.ModeSettings.config",
                    "template.config",
                    "Yamarket.xml",
                    "robots.txt",
                    "sitemap.html",
                    "sitemap.xml",
                    "combined_",
                    "install.txt",
                    "website.publishproj",
                    "app_offline.html",
                    "app_offline.htm"
                )

   usingClose ($outputFile = New-Object System.IO.StreamWriter($fileCodeMask, $false, [System.Text.Encoding]::UTF8)) {
	foreach ($advFileName in $allFiles){
		$fileName = $advFileName.Replace($sourcePath + "\", "");
        $any= $false;
        foreach($exclude in $ExclusionFoldersAndFiles)
        {
            if ($fileName.Contains($exclude))
            {
                $any= $true;
                break;
            }
        }
        if ($any -eq $true){
            Write-Host "skip " $fileName
            continue
        }
		
		#skip some
		usingClose($hashAlg = New-Object System.Security.Cryptography.SHA1Managed) {
		    usingClose($file=New-Object System.IO.FileStream($advFileName,[System.IO.FileMode]::Open, [System.IO.FileAccess]::Read)) {
		        $hash = $hashAlg.ComputeHash($file)
		        $outputFile.WriteLine($fileName+";"+[System.BitConverter]::ToString($hash))
		        }
		    }
	    }
	}
}
Export-ModuleMember -Function  *