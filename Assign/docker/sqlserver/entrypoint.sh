$entrypoint = @'
#!/bin/bash
set -e

/usr/local/bin/restore.sh &

exec /opt/mssql/bin/sqlservr
'@ -replace "`r`n", "`n"

[System.IO.File]::WriteAllText("F:\Semester\Assign\docker\sqlserver\entrypoint.sh", $entrypoint)