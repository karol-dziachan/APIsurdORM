﻿IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE NAME = 'dbo'))
BEGIN
	EXEC ('CREATE SCHEMA [dbo] AUTHORIZATION [dbo]')
END

--__TABLES_DDL__