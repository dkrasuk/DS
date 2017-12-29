param (
    [string]$folder = '', #'C:\dictionary\',
    [string]$apiUri = '' #'http://localhost/DictionaryApi/dictionary/v1/'
)

Function LogWrite
{
   Param ([string]$logstring)
   $Logfile = $folder + "uplodDictionaries.log"
   Add-content $Logfile -value $logstring
}

if ($folder -eq '') {
    #throw 'Parameter $folder not set!'
    LogWrite 'Parameter $folder not set! Defoult value is "./"'
	$folder = './'
}

if ($apiUri -eq '') {
    throw 'Parameter $apiUri not set!'
}

$files = Get-ChildItem $folder -Filter *.json

for ($i=0; $i -lt $files.Count; $i++) {
    $file = $files[$i].FullName
    try
    {
        $result = Invoke-WebRequest -Uri $apiUri -Method PUT -InFile $file -ContentType "application/json; charset=utf-8" -UseDefaultCredentials
        $logMessage = [string]$result.statuscode + ' : ' + $file + ' : ' + $result.StatusDescription
        LogWrite $logMessage
    }
    catch [System.Net.WebException] 
    {
        $statusCode = [string]$_.Exception.Response.StatusCode
        $html = $_.Exception.Response.StatusDescription
        $logMessage = $statusCode + ' : ' + $file + ' : ' + $html
        LogWrite $logMessage

        $result = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($result)
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd();
        LogWrite $responseBody
    }
}


