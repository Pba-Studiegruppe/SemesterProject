$restore = @'
#!/bin/bash
set -e

SQLCMD="/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P ${MSSQL_SA_PASSWORD} -C"
DB_NAME="AssignDB"
BAK_PATH="/var/opt/mssql/backup/AssignDB.bak"

echo "[restore] Venter paa at SQL Server bliver tilgaengelig..."
for i in {1..60}; do
    if $SQLCMD -Q "SELECT 1" >/dev/null 2>&1; then
        echo "[restore] SQL Server er oppe."
        break
    fi
    sleep 2
done

DB_EXISTS=$($SQLCMD -h -1 -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = '${DB_NAME}'" | tr -d '[:space:]')

if [ "$DB_EXISTS" = "1" ]; then
    echo "[restore] ${DB_NAME} findes allerede - springer restore over."
    exit 0
fi

if [ ! -f "$BAK_PATH" ]; then
    echo "[restore] ADVARSEL: ingen backup fundet paa ${BAK_PATH} - springer over."
    exit 0
fi

echo "[restore] Laeser logiske filnavne fra backup..."
LOGICAL=$($SQLCMD -h -1 -W -Q "SET NOCOUNT ON; RESTORE FILELISTONLY FROM DISK = N'${BAK_PATH}'")

DATA_NAME=$(echo "$LOGICAL" | awk '$3=="D"{print $1; exit}')
LOG_NAME=$(echo "$LOGICAL"  | awk '$3=="L"{print $1; exit}')

echo "[restore] Data='${DATA_NAME}' Log='${LOG_NAME}'. Koerer RESTORE..."
$SQLCMD -Q "RESTORE DATABASE [${DB_NAME}] FROM DISK = N'${BAK_PATH}' WITH MOVE N'${DATA_NAME}' TO N'/var/opt/mssql/data/${DB_NAME}.mdf', MOVE N'${LOG_NAME}' TO N'/var/opt/mssql/data/${DB_NAME}_log.ldf', REPLACE, RECOVERY"

echo "[restore] ${DB_NAME} restored."
'@ -replace "`r`n", "`n"

[System.IO.File]::WriteAllText("F:\Semester\Assign\docker\sqlserver\restore.sh", $restore)