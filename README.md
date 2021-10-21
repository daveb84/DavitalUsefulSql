# DavitalUsefulSql
A SQL Server Database project with some useful SQL CLR functions.  Requires Visual Studio with SQL Server 
Data Tools (SSDT) installed.

## Setup / Deployment

**AdHoc/SqlClr.sql**  
Run on SQL Server instance to enable SQL CLR

**UsefulSql.publish.xml**  
Publishes a database called `UsefulSql` to SQL Instance `localhost\SQLEXPRESS`

## Functions
**dbo.GetFiles**  
Returns a list of all files in a directory (including all subfolders)
```SQL
SELECT * FROM UsefulSql.dbo.GetFiles('c:\myfolder')
```

**dbo.GetFileContents**  
Returns the contents of a file
```SQL
SELECT UsefulSql.dbo.GetFileContent('c:\myfolder\myfile.txt')
```

**dbo.GetUnicodeValues**  
Returns all characers from a string with their unicode integer values.
```SQL
SELECT *
FROM 
	UsefulSql.dbo.GetUnicodeValues('some text')
```

**dbo.ReplaceUnicodeValues**  
Returns a string with one character code replaced with another. 
```SQL
-- space = 32
-- underscore = 95
-- returns replace_spaces_with_underscores
SELECT UsefulSql.dbo.ReplaceUnicodeValues('replace spaces with underscores', 32, 95)	
```