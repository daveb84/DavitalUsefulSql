using System.Data.SqlTypes;
using static UserDefinedFunctions;

namespace DavitalUsefulSql.Tests
{
    [TestClass]
    public class GetTextLinesTests
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                yield return new object[] 
                {
                    "1\r\n2",
                    new[]
                    {
                        ("1", false),
                        ("2", false),
                    }
                };

                yield return new object[]
                {
                    "1\n2",
                    new[]
                    {
                        ("1", false),
                        ("2", false),
                    }
                };

                yield return new object[]
                {
                    "1\n2\r\n3",
                    new[]
                    {
                        ("1", false),
                        ("2", false),
                        ("3", false),
                    }
                };
                yield return new object[]
                {
                    "1\r\n\r\n3",
                    new[]
                    {
                        ("1", false),
                        ("", true),
                        ("3", false),
                    }
                };
                yield return new object[]
                {
                    "1\r\n \r\n3",
                    new[]
                    {
                        ("1", false),
                        (" ", true),
                        ("3", false),
                    }
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(TestCases))]
        public void GetFileLines_ReturnsValues(string contents, (string line, bool isWhiteSpace)[] expected)
        {
            var result = UserDefinedFunctions.GetTextLines(new SqlString(contents));

            var results = result.Cast<LineResult>().ToList();
            Assert.AreEqual(expected.Length, results.Count);

            for (var i = 0; i < expected.Length; i++)
            {
                var actual = results[i];
                var expectedLine = expected[i];

                Assert.AreEqual(i + 1, actual.LineNumber);
                Assert.AreEqual(expectedLine.line, actual.Line);
                Assert.AreEqual(expectedLine.isWhiteSpace, actual.IsWhiteSpace);
            }
        }
    }
}