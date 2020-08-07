$url = "http://localhost:53137"
$username = "02102002"
$password = "giorgia2"

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/x-www-form-urlencoded")

$body = "grant_type=password&username=$username&password=$password"

$response = Invoke-RestMethod $url'//Token' -Method 'POST' -Headers $headers -Body $body 
$response | ConvertTo-Json

$accessToken = $response.access_token
$bearer = "Bearer $accessToken"

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization",  $bearer)

$response = Invoke-RestMethod $url'/api/TipiProfilo' -Method 'GET' -Headers $headers
$response | ConvertTo-Json


# https://www.it-swarm.dev/it/c%23/autenticazione-jwt-api-web-asp.net/827483858/
# https://github.com/cuongle/WebApi.Jwt