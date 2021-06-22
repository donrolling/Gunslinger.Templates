$projectDirectory = 'C:\Projects\Gunslinger.Templates\SampleProject\'
$gunslingerDirectory = $projectDirectory + 'Gunslinger\'
$pathToScripts = $gunslingerDirectory + 'Scripts\'
######################################################################################################
# Gunslinger Config ##################################################################################
######################################################################################################
$gunslingerExePath = $gunslingerDirectory + 'Gunslinger.Console.exe'
$generationContextPath = $gunslingerDirectory + 'GenerationContext.json'
$outputPath = 'C:\Projects\Gunslinger.Templates\SampleProject'
$templatesPath = 'C:\Projects\Gunslinger.Templates\SampleProject\Gunslinger\Templates'
$processTemplateStubs = 'true'
######################################################################################################
# DAC Config #########################################################################################
######################################################################################################
$dacpacScriptPath = $pathToScripts + 'DeployViaDAC.ps1'
$dacpacFilePath = $projectDirectory + 'sample.database\bin\Debug\sample.database.dacpac'
$sqlPackagePath = 'C:\SQLPackage\Microsoft.SqlServer.Dac.dll'
$sqlServerName = 'localhost'
$databaseName = 'sample'
######################################################################################################
# Ad-Hoc SQL Config ##################################################################################
######################################################################################################
$adHocScriptPath = $pathToScripts + 'RunAdHocScripts.ps1'
$adHocScripts #= ('test1.sql', 'test2.sql')

######################################################################################################
# Gunslinger Execution ###############################################################################
######################################################################################################
Write-Host 'Running Gunslinger now'
Write-Host ''    
& $gunslingerExePath $generationContextPath $outputPath $templatesPath $processTemplateStubs
Write-Host ''

######################################################################################################
# DAC Execution ######################################################################################
######################################################################################################
Write-Host 'You can run a DAC update for your database now. You may need to update and build the sql project that outputs the DAC file first.'
$runDacUpdate = (Read-Host 'Run DACUpdate now? y/n').ToUpper()
if ($runDacUpdate -eq 'Y') 
{
    Write-Host 'Running DacPac'
    & $dacpacScriptPath -sqlPackagePath $sqlPackagePath -sqlServerName $sqlServerName -databaseName $databaseName -dacpacPath $dacpacFilePath
}
Write-Host ''

######################################################################################################
# Post Update Script Execution #######################################################################
######################################################################################################
if (!$adHocScripts -or $adHocScripts.Length -eq 0)
{
    Write-Host 'Skipping Running Post Update Scripts because there are none to run right now.'
}
else 
{
    $runPostUpdateScripts = (Read-Host 'Run Post Update Scripts now? y/n').ToUpper()
    if ($runPostUpdateScripts -eq 'Y') 
    {
        Write-Host "Running ($adHocScripts.Length) Post Update Scripts"
        & $adHocScriptPath -sqlServerName $sqlServerName -databaseName $databaseName -pathToScripts $pathToScripts.TrimEnd('\\') -scripts $adHocScripts
    }
} 

Write-Host ''
Write-Host 'Complete!'
Write-Host ''
