$url = "http://localhost:56465"
$username = "02102002"
$password = "giorgia2"

# Recupero token
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/x-www-form-urlencoded")

$body = "grant_type=password&username=$username&password=$password"

$response = Invoke-RestMethod $url'/Token' -Method 'POST' -Headers $headers -Body $body 
$response | ConvertTo-Json

$accessToken = $response.access_token
$bearer = "Bearer $accessToken"


# Chiamata REST protetta da autorizzazioni
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization",  $bearer)

$response = Invoke-RestMethod $url'/api/Values?' -Method 'GET' -Headers $headers 
$response | ConvertTo-Json