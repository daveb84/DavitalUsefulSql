using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlString GetXPathValue(SqlString xml, SqlString xpath)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml.Value);

        var node = xmlDoc.SelectSingleNode(xpath.Value);

        if (node?.Value == null)
        {
            return SqlString.Null;
        }

        return new SqlString(node.Value);
    }

    [SqlFunction(DataAccess = DataAccessKind.None,
        TableDefinition = "Value NVARCHAR(MAX)",
        FillRowMethodName = "GetXPathValuesRow")]
    public static IEnumerable GetXPathValues(SqlString xml, SqlString xpath)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml.Value);

        var nodes = xmlDoc.SelectNodes(xpath.Value);

        foreach (XmlNode node in nodes)
        {
            yield return node.Value;
        }
    }

    public static void GetXPathValuesRow(object result, out SqlString value)
    {
        value = result as string;
    }
}
