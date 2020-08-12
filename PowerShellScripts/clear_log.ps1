# $url = "https://lattemarche.azurewebsites.net/"
$url = "http://robertoborsini.myqnapcloud.com:81/"
$username = "02102002"
$password = "giorgia2"

# Recupero Token
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/x-www-form-urlencoded")

$body = "grant_type=password&username=$username&password=$password"

$response = Invoke-RestMethod $url'//Token' -Method 'POST' -Headers $headers -Body $body 
$response | ConvertTo-Json

$accessToken = $response.access_token
$bearer = "Bearer $accessToken"

# POST Logs/Clear
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization",  $bearer)

$response = ""
$response = Invoke-RestMethod $url'/api/Logs/Clear' -Method 'POST' -Headers $headers
$response | ConvertTo-Json    


Write-Output "Done"