﻿sp_configure 'clr enabled', 1
go
RECONFIGURE
go
sp_configure 'clr enabled'
go

EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;

EXEC sp_configure 'clr strict security', 0;
RECONFIGURE;