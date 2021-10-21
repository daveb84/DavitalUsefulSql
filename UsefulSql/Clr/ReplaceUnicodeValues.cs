using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlString ReplaceUnicodeValues(SqlString content, SqlInt32 find, SqlInt32 replace)
    {
        if (content.IsNull)
        {
            return SqlString.Null;
        }

        var chars = content.Value.ToCharArray();
        var replaced = new List<char>();
        var findChar = (char)find.Value;
        var replaceChar = (char)replace.Value;

        foreach (var c in chars)
        {
            if (c == findChar)
            {
                replaced.Add(replaceChar);
            }
            else
            {
                replaced.Add(c);
            }
        }

        var replacedValue = new string(replaced.ToArray());

        return new SqlString(replacedValue);
    }
}
