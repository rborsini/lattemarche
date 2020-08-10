# https://ironmansoftware.com/automated-web-testing-with-selenium-and-powershell/

# Install-Module -Name Selenium -RequiredVersion 1.2

$url = "http://robertoborsini.myqnapcloud.com:81"

$Driver = Start-SeChrome

# Login -> Dashboard
Enter-SeUrl -Driver $Driver -Url $url"/account/login"

$Username = Find-SeElement -Driver $Driver -Id 'Username'
Send-SeKeys -Element $Username -Keys "02102002"

$Password = Find-SeElement -Driver $Driver -Id 'Password'
Send-SeKeys -Element $Password -Keys "giorgia2"

Find-SeElement -Driver $Driver -Id 'btnLogin' | Invoke-SeClick


# Utenti
Enter-SeUrl -Driver $Driver -Url $url"/utenti"

# Dispositivi mobili
Enter-SeUrl -Driver $Driver -Url $url"/dispositivi"

# Giri
Enter-SeUrl -Driver $Driver -Url $url"/giri"

# Prelievi latte
Enter-SeUrl -Driver $Driver -Url $url"/prelievi"

# Analisi latte
Enter-SeUrl -Driver $Driver -Url $url"/analisilatte"