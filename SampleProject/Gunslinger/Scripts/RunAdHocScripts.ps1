param([string]$sqlServerName, [string]$databaseName, [string]$pathToScripts, [string[]]$scripts)
Import-Module Sqlps -DisableNameChecking;
#Example to test if SQL snap-ins are present
    #get-pssnapin -Registered
#Example call to pass multiple scripts
    #.\RunAdHocScripts.ps1 -sqlServerName 'localhost' -databaseName 'Test' -pathToScripts 'C:\Projects\idi.utilities\Gunslinger.Templates\SampleProject\Gunslinger\Scripts' -scripts ('test.sql', 'test.sql')
foreach ($script in $scripts) {
    try {
        $path = "$pathToScripts\$script"
        Write-Host $path
        invoke-sqlcmd -inputfile $path -serverinstance $sqlServerName -database $databaseName | out-null
    } catch {
        Write-Host "Run Script ($script) Exception"
        $Error | format-list -force;
        Write-Host $Error[0].Exception.ParentContainsErrorRecordException
        #throw $Error[0].Exception.ParentContainsErrorRecordException;
    }
}
