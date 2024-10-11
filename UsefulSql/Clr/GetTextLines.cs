using System.Collections;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    private static readonly Regex splitLines = new Regex(@"\r?\n", RegexOptions.Compiled);

    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "LineNumber INT, Line NVARCHAR(MAX), IsWhiteSpace BIT",
        FillRowMethodName = "GetTextLinesFillRow")]
    public static IEnumerable GetTextLines(SqlString contents)
    {
        if (string.IsNullOrEmpty(contents.Value)) {
            return new object[0];
        }

        var split = splitLines.Split(contents.Value);

        return GetLines(split);
    }

    private static IEnumerable GetLines(string[] lines)
    {
        for (var i = 0; i < lines.Length; i++) 
        { 
            yield return new LineResult(i + 1, lines[i]);
        }
    }

    public class LineResult
    {
        public LineResult(int lineNumber, string line)
        {
            LineNumber = lineNumber;
            Line = line;
        }

        public int LineNumber { get; }
        public string Line { get; }
        public bool IsWhiteSpace => string.IsNullOrWhiteSpace(Line);
    }

    public static void GetTextLinesFillRow(object result, out SqlInt32 lineNumber, out SqlString line, out SqlBoolean isWhiteSpace)
    {
        var entry = (LineResult)result;
        lineNumber = entry.LineNumber;
        line = entry.Line;
        isWhiteSpace = entry.IsWhiteSpace;
    }
}
