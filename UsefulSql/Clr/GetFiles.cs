using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "FullName NVARCHAR(2000), Name NVARCHAR(255), Directory NVARCHAR(2000), Extension NVARCHAR(20), IsDirectory BIT, Depth INT, Size BIGINT",
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
            yield return new FileResult(directory, file, depth);
        }

        var childDepth = depth + 1;
        foreach(var child in directory.GetDirectories())
        {
            yield return new FileResult(directory, child, depth);

            var children = GetChildren(child, childDepth);

            foreach (var grandChild in children)
            {
                yield return grandChild;
            }
        }
    }

    public class FileResult
    {
        public FileResult(DirectoryInfo directory, FileSystemInfo result, int depth)
        {
            this.Directory = directory;
            this.Result = result;
            this.Depth = depth;
        }

        public DirectoryInfo Directory { get; }
        public FileSystemInfo Result { get; }
        public int Depth { get; }
    }

    public static void GetFilesFillRow(object result, out SqlString fullName, out SqlString name, out SqlString directory, out SqlString extension, out SqlBoolean isDirectory, out SqlInt32 depth, out SqlInt64 size)
    {
        var entry = (FileResult)result;
        var file = entry.Result;

        fullName = file.FullName;
        directory = entry.Directory.FullName;
        name = file.Name;
        extension = file.Extension;
        isDirectory = file is DirectoryInfo;
        depth = entry.Depth;
        size = (file as FileInfo)?.Length ?? 0;
    }
}
