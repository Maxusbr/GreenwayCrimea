
Function backupDb()
{
	    param (
            [string] $sqlName,
            [string] $dbname,
            [string] $backupPath
    
            )	

	#Load the required assemlies SMO and SmoExtended.
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null 
	
	# Connect SQL Server.
	$sqlServer = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $sqlName
	
	#Create SMO Backup object instance with the Microsoft.SqlServer.Management.Smo.Backup
	$dbBackup = New-Object ("Microsoft.SqlServer.Management.Smo.Backup")
	
	$dbBackup.Database = $dbname
	$dbBackup.Action = [Microsoft.SqlServer.Management.Smo.BackupActionType]::Database

	$bkpDevice = New-Object ('Microsoft.SqlServer.Management.Smo.BackupDeviceItem') ($backupPath, [Microsoft.SqlServer.Management.Smo.DeviceType]::File);
	
	#Add the backup file to the Devices
	$dbBackup.Devices.Add($bkpDevice)
		
	#Call the SqlBackup method to complete backup 
	$dbBackup.SqlBackup($sqlServer)
	
	Write-Host "...Backup of the database"$dbname" completed..."
}


Function  SqlStringExec
{	
    param
    (
    [string]$serverinstance,
    [string]$database,
    [string]$fullpath
    )
	
	#Load the required assemlies SMO and SmoExtended.
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null 
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.BatchParserClient") | Out-Null
	
    $sqlServer = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $serverinstance
    $db = $sqlServer.Databases.Item($database);
    $reader = New-Object System.IO.StreamReader($fullpath)
    $script = $reader.ReadToEnd()
    $db.ExecuteNonQuery($script)
	#invoke-sqlcmd -serverinstance $serverinstance -database $database -inputFile $file
}

Function  SqlCommandExec
{	
    param
    (
    [string]$serverinstance,
    [string]$database,
    [string]$command
    )
	
	#Load the required assemlies SMO and SmoExtended.
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
	[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null 
	
    $sqlServer = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $serverinstance
    $db = $sqlServer.Databases.Item($database);
    $db.ExecuteNonQuery($command)
	#invoke-sqlcmd -serverinstance $serverinstance -database $database -inputFile $file
}


function CreateDb
{
    param (
            [string] $sqlName,
            [string] $databaseName,
            [string] $dbPath
    
            )
    #Load the required assemlies SMO and SmoExtended.
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null

    $sqlServer = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $sqlName

	$dbExist = $sqlServer.Databases[$databaseName]
	if ($dbExist -ne $null) {
		return
	}
	
    $db = New-Object ('Microsoft.SqlServer.Management.Smo.Database') ($sqlName, $databaseName)
    $fg = New-Object ('Microsoft.SqlServer.Management.Smo.FileGroup') ($db, "PRIMARY")
    $dataFile =  New-Object ('Microsoft.SqlServer.Management.Smo.DataFile') ($fg, $databaseName)
    $dataFile.FileName = $dbPath + "\" + $databaseName +".mdf"
    $dataFile.GrowthType =[Microsoft.SqlServer.Management.Smo.FileGrowthType]::KB 
    $dataFile.Growth=10000
    $dataFile.IsPrimaryFile = $true
    $fg.Files.Add($dataFile)

    $logname = $databaseName + "_log"

    $logFile =  New-Object ('Microsoft.SqlServer.Management.Smo.LogFile') ($db, $logname)
    $logFile.FileName = $dbPath + "\" + $databaseName +"_log" + ".ldf"
    $logFile.GrowthType =[Microsoft.SqlServer.Management.Smo.FileGrowthType]::Percent 
    $logFile.Growth=10    
    $db.LogFiles.Add($logFile)

    $db.FileGroups.Add($fg);
    $db.RecoveryModel = [Microsoft.SqlServer.Management.Smo.RecoveryModel]::Simple;
    $db.Create();
    $sqlServer.Refresh();    

}

function RestoreDb
{
    param (
            [string] $sqlName,
            [string] $databaseName,
            [string] $dbPath,
            [string] $backupFile
            )
    #Load the required assemlies SMO and SmoExtended.
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null

    CreateDb $sqlName $databaseName $dbPath	
    $sqlServer = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $sqlName
	$sqlServer.KillAllProcesses($databaseName)

    $dbRestore = new-object ("Microsoft.SqlServer.Management.Smo.Restore")
    $dbRestore.Action = [Microsoft.SqlServer.Management.Smo.RestoreActionType]::Database
    $dbRestore.Database=$databaseName
    $dbRestore.NoRecovery = $false
    $backupDevice = New-Object("Microsoft.SqlServer.Management.Smo.BackupDeviceItem") ($backupFile, "File")
    $dbRestore.Devices.Add($backupDevice)

    $fl = $dbRestore.ReadFileList($sqlServer);
    foreach($row in $fl)
    {
        $relocatFile = New-Object("Microsoft.SqlServer.Management.Smo.RelocateFile")
        $relocatFile.LogicalFileName=$row["LogicalName"]
        $fileType = $row["Type"].ToUpper()
        if($fileType.Equals("D")){
            $relocatFile.PhysicalFileName = $dbPath + "\" + $databaseName + ".mdf"
        }
        else{
            $relocatFile.PhysicalFileName = $dbPath + "\" + $databaseName + "_log" + ".ldf"
        }
        $dbRestore.RelocateFiles.Add($relocatFile);
    }
     $dbRestore.ReplaceDatabase = $true;
     $dbRestore.SqlRestore($sqlServer);
}

Export-ModuleMember -Function  *