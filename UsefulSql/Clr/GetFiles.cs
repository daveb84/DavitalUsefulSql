using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "FullName NVARCHAR(2000), Name NVARCHAR(255), Extension NVARCHAR(20), IsDirectory BIT, Depth INT",
        FillRowMethodName = "GetFilesFillRow")]
    public static IEnumerable GetFiles(SqlString directory)
    {
        var di = new DirectoryInfo(directory.Value);

        if (di.Exists)
        {
            return GetChildren(di, 0);
        }

        return new object[0];
    }

    private static IEnumerable<FileResult> GetChildren(DirectoryInfo directory, int depth)
    {
        foreach (var file in directory.GetFiles())
        {
            yield return new FileResult { Result = file, Depth = depth };
        }

        var childDepth = depth + 1;
        foreach(var child in directory.GetDirectories())
        {
            yield return new FileResult { Result = child, Depth = depth };

            var children = GetChildren(child, childDepth);

            foreach (var grandChild in children)
            {
                yield return grandChild;
            }
        }
    }

    public class FileResult
    {
        public FileSystemInfo Result { get; set; }

        public int Depth { get; set; }
    }

    public static void GetFilesFillRow(object result, out SqlString fullName, out SqlString name, out SqlString extension, out SqlBoolean isDirectory, out SqlInt32 depth)
    {
        var entry = (FileResult)result;
        var file = entry.Result;

        fullName = file.FullName;
        name = file.Name;
        extension = file.Extension;
        isDirectory = file is DirectoryInfo;
        depth = entry.Depth;
    }
}
