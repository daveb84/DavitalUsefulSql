using System.Data.SqlTypes;

namespace DavitalUsefulSql.Tests
{
    [TestClass]
    public class GetXPathTests
    {
        public static IEnumerable<object[]> GetXPathTestCases
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            get
            {
                yield return new object[] 
                {
                    @"<root><elm key=""match"" value=""aaa"" /><elm key=""not match"" value=""bbb"" /></root>",
                    @"//root/elm[@key=""match""]/@value", 
                    "aaa"
                };

                yield return new object[]
                {
                    @"<root><elm key=""match"" value=""aaa"" /><elm key=""match"" value=""bbb"" /></root>",
                    @"//root/elm[@key=""match""]/@value",
                    "aaa"
                };

                yield return new object[]
                {
                    @"<root><elm key=""something"" value=""aaa"" /></root>",
                    @"//root/elm[@key=""no match""]/@value",
                    null
                };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetXPathTestCases))]
        public void GetXPathTest_ReturnsValue(string xml, string xpath, string expected)
        {
            var result = UserDefinedFunctions.GetXPathValue(new SqlString(xml), new SqlString(xpath));

            if (expected == null)
            {
                Assert.IsTrue(result.IsNull);
            }
            else
            {
                Assert.AreEqual(expected, result.Value);
            }
        }
    }
}