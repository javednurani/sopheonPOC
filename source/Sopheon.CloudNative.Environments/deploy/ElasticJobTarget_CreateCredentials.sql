IF NOT EXISTS(SELECT name FROM sys.sql_logins WHERE name = 'masteruser')
	BEGIN
		CREATE LOGIN masteruser WITH PASSWORD='^Enigma^'
		CREATE USER masteruser FROM LOGIN masteruser
	END
ELSE 
	BEGIN
		PRINT 'masteruser already exists'
	END

IF NOT EXISTS(SELECT name FROM sys.sql_logins WHERE name = 'jobuser')
	BEGIN
		CREATE LOGIN jobuser WITH PASSWORD='^Enigma^'		
	END
ELSE 
	BEGIN
		PRINT 'jobuser already exists'
	END


