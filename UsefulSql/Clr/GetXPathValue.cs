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
}
