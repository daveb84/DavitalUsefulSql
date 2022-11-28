using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlBoolean RegexIsMatch(SqlString value, SqlString pattern, SqlInt32 regexOptions)
    {
        var regex = GetRegex(pattern, regexOptions);

        return regex.IsMatch(value.Value);
    }

    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "Success BIT, Value NVARCHAR(MAX), Position INT",
        FillRowMethodName = "RegexGetMatchRow")]
    public static IEnumerable RegexGetMatch(SqlString value, SqlString pattern, SqlInt32 regexOptions)
    {
        var regex = GetRegex(pattern, regexOptions);

        var match = regex.Match(value.Value);

        return new object[] { match };
    }

    public static void RegexGetMatchRow(object result, out SqlBoolean success, out SqlString value, out SqlInt32 position)
    {
        var match = (Match)result;

        success = match.Success;
        value = match.Value;
        position = match.Index;
    }

    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "Value NVARCHAR(MAX), Position INT",
        FillRowMethodName = "RegexGetMatchesRow")]
    public static IEnumerable RegexGetMatches(SqlString value, SqlString pattern, SqlInt32 regexOptions)
    {
        var regex = GetRegex(pattern, regexOptions);

        var matches = regex.Matches(value.Value);

        return matches;
    }

    public static void RegexGetMatchesRow(object result, out SqlString value, out SqlInt32 position)
    {
        var match = (Match)result;

        value = match.Value;
        position = match.Index;
    }

    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "MatchValue NVARCHAR(MAX), MatchPosition INT, GroupName NVARCHAR(MAX), GroupValue NVARCHAR(MAX), GroupPosition INT",
        FillRowMethodName = "RegexGetGroupsRow")]
    public static IEnumerable RegexGetGroups(SqlString value, SqlString pattern, SqlInt32 regexOptions)
    {
        var regex = GetRegex(pattern, regexOptions);

        var matchResults = new List<object>();

        var matches = regex.Matches(value.Value);

        foreach (Match match in matches)
        {
            foreach (Group group in match.Groups)
            {
                matchResults.Add(new Tuple<Match, Group>(match, group));
            }
        }

        return matchResults;
    }

    public static void RegexGetGroupsRow(object result, out SqlString matchValue, out SqlInt32 matchPosition, out SqlString groupName, out SqlString groupValue, out SqlInt32 groupPosition)
    {
        var resultMatch = (Tuple<Match, Group>)result;
        var match = resultMatch.Item1;
        RegexGetMatchesRow(match, out matchValue, out matchPosition);

        var group = resultMatch.Item2;
        groupName = group.Name;
        groupValue = group.Value;
        groupPosition = group.Index;
    }

    private static Regex GetRegex(SqlString pattern, SqlInt32 regexOptions)
    {
        var regex = new Regex(pattern.Value, regexOptions.IsNull ? RegexOptions.None : (RegexOptions)regexOptions.Value);

        return regex;
    }
}
