@REM cambio directory mysql
cd C:\Program Files\Microsoft SQL Server\150\DAC\bin

@REM esportazione database
SqlPackage.exe /a:export /scs:"data source=lattemarche.database.windows.net;initial catalog=lattemarche;persist security info=False;user id=lattemarche;password=Whitemilk01!;" /tf:"C:\Projects\backup\lattemarche\lattemarche_%date:/=-%.bacpac"

@REM spostamento file
xcopy C:\Projects\backup\lattemarche\*.bacpac C:\Users\Rob\OneDrive\we-code\clienti\LatteMarche\backup /Y
del C:\Projects\backup\lattemarche\*.bacpac



