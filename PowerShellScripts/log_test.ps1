function Log ($text) {
    $date = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    Add-Content log.txt "$date - $text"
    Write-Output "$date - $text"
}


Log "Prima riga"
Log "Seconda riga"