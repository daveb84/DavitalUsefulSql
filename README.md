# DavitalUsefulSql
A SQL Server Database project with some useful SQL CLR functions.  Requires Visual Studio with SQL Server 
Data Tools (SSDT) installed.

## Setup / Deployment

**AdHoc/SqlClr.sql**  
Run on SQL Server instance to enable SQL CLR

**UsefulSql.publish.xml**  
Publishes a database called `UsefulSql` to SQL Instance `localhost\SQLEXPRESS`

## Functions

### File system functions
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

### Character encoding functions
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

### Regex functions
The following regex related functions all have the following parameters

| Param | Description |
|-|-|
| value | The string value to search |
| pattern | The regular expression |
| regexOptions | Regular expression options. Integer value that will be converted to `System.Text.RegularExpressions.RegexOptions`. Can be supplied as `NULL`, which will convert to `RegexOptions.None` |

**dbo.RegexGetMatches**  
Wrapper around [Regex.Matches](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.matches?view=net-6.0). Returns a row for each match.

```SQL
SELECT * 
FROM 
	UsefulSql.dbo.RegexGetMatches('Extract individual words', '\b(\w+)\b', NULL)
```

**dbo.RegexGetMatch**  
Wrapper around [Regex.Match](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.match?view=net-6.0). Always returns a single row with the match result.

```SQL
SELECT * 
FROM 
	UsefulSql.dbo.RegexGetMatch('Gets first match only', '\b(\w+)\b', NULL)
```

**dbo.RegexIsMatch**  
Wrapper around [Regex.IsMatch](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.ismatch?view=net-6.0). Scalar function, returning a BIT indicating if the expression matches.

```SQL
SELECT UsefulSql.dbo.RegexIsMatch('Returns 1 or 0', '\b(\w+)\b', NULL)
```

### XML functions

**dbo.GetXPathValue**  
Returns the first matching result of an XPath expression for the given XML.

```SQL
SELECT UsefulSql.dbo.GetXPathValue('<root><item key="1" value="AAA" /></root>', '//root/item[@key="1"]/@value')
```

**dbo.GetXPathValues**  
Returns all matching results of an XPath expression for the given XML.

```SQL
SELECT *
FROM
	UsefulSql.dbo.GetXPathValues('<root><item key="1" value="AAA" /><item key="2" value="BBB" /></root>', '//root/item/@value')
```