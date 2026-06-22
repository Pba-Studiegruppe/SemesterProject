# syntax=docker/dockerfile:1

# =============================================================================
# SQL Server 2022 med automatisk RESTORE af AssignDB.bak ved opstart.
#
# MSSQL-imaget har INGEN auto-init-mappe (i modsætning til postgres/mysql).
# En .bak kan derfor ikke bare "smides ind". Vi pakker derfor backup'en +
# et entrypoint-script ind i imaget, der venter på at serveren er oppe og
# kører RESTORE idempotent (kun hvis databasen ikke allerede findes).
# =============================================================================
FROM mcr.microsoft.com/mssql/server:2022-latest

# Skift til root for at kunne kopiere filer ind og sætte rettigheder.
USER root

# Mappe til backup-filen.
RUN mkdir -p /var/opt/mssql/backup

# Kopiér backup'en ind i imaget. Forventer at AssignDB.bak ligger i
# build-context'ets rod (samme niveau som docker-compose.yml).
COPY AssignDB.bak /var/opt/mssql/backup/AssignDB.bak

# Kopiér scripts ind.
COPY docker/sqlserver/entrypoint.sh /usr/local/bin/entrypoint.sh
COPY docker/sqlserver/restore.sh   /usr/local/bin/restore.sh

# Normalisér linjeendelser (Windows CRLF -> LF) og gør scripts eksekverbare.
# Dette gør imaget robust selv hvis filerne er gemt med Windows-linjeendelser,
# som ellers ville få bash til at fejle med '\r'-fejl ved kørsel.
RUN sed -i 's/\r$//' /usr/local/bin/entrypoint.sh /usr/local/bin/restore.sh \
    && chmod +x /usr/local/bin/entrypoint.sh /usr/local/bin/restore.sh \
    && chown -R mssql:root /var/opt/mssql/backup

# Tilbage til den indbyggede non-root mssql-bruger.
USER mssql

# Vores eget entrypoint starter SQL Server OG kører restore i baggrunden.
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]