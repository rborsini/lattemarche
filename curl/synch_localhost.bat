set client_id=02102002
set password=giorgia2

curl -X POST ^
http://localhost:50364/Token ^
 -H "Cache-Control: no-cache" ^
 -H "Content-Type: application/x-www-form-urlencoded" ^
 -d "grant_type=password&username=%client_id%&password=%password%" ^
 > curl-oauth-result.txt

jq -r ".access_token" curl-oauth-result.txt > bearer.txt
set /p bearer= < bearer.txt
echo %bearer%

curl -X POST ^
http://localhost:50364/api/PrelieviLatte/Synch ^
 -H "Authorization: Bearer %bearer%" ^
 -H "Cache-Control: no-cache" ^
 -H "Content-Type: application/x-www-form-urlencoded" ^
 -H "Content-Length:0" ^
 > response.txt

 