using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "Character NVARCHAR(1), CharacterCode INT, CharacterCodeHex NVARCHAR(10)",
        FillRowMethodName = "GetUnicodeValuesFillRow")]
    public static IEnumerable GetUnicodeValues(SqlString value)
    {
        if (value.IsNull)
        {
            return new object[0];
        }

        return (value.Value ?? string.Empty).ToCharArray();
    }

    public static void GetUnicodeValuesFillRow(object result, out SqlString character, out SqlInt32 characterCode, out SqlString characterCodeHex)
    {
        var c = (char)result;
        var code = (int)c;

        character = new SqlString(c.ToString());
        characterCode = new SqlInt32(code);
        characterCodeHex = new SqlString(code.ToString("X"));
    }
}
