using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlString GetFileContents(SqlString filePath)
    {
        return File.ReadAllText(filePath.Value);
    }
}
