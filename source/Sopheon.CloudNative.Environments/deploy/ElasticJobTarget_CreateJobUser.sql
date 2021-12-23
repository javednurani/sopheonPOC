IF NOT EXISTS(SELECT name FROM sys.database_principals WHERE name = 'jobuser')
    BEGIN
		PRINT 'No User Found. Creating one'
		CREATE USER jobuser FROM LOGIN jobuser
		GRANT ALTER ON SCHEMA::dbo TO jobuser
		GRANT CREATE TABLE TO jobuser
		GRANT CREATE SCHEMA TO jobuser
	END
ELSE 
    BEGIN
		PRINT 'Jobuser found'
	END