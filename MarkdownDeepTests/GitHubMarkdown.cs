using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MarkdownDeep;
using System.Reflection;

namespace MarkdownDeepTests
{
    [TestFixture]
    class GitHubMarkdownTests
    {
        public static IEnumerable<TestCaseData> GetTests()
        {
            return Utils.GetTests("githubmarkdown");
        }

        [Test, TestCaseSource("GetTests")]
        public void Test_githubmarkdown(string resourceName)
        {
            Utils.RunResourceTest(resourceName);
        }

        [Test]
        public void BreakInOrderedList()
        {
            string input = TestCases.breaklist_md;
            string expected = TestCases.breaklist_html;

            var md = new MarkdownDeep.Markdown();

            string actual = md.Transform(input);
            string actual_clean = Utils.strip_redundant_whitespace(actual);
            string expected_clean = Utils.strip_redundant_whitespace(expected);

            string sep = new string('-', 30) + "\n";

            Console.WriteLine("Input:\n" + sep + input);
            Console.WriteLine("Actual:\n" + sep + actual);
            Console.WriteLine("Expected:\n" + sep + expected);

            Assert.AreEqual(expected_clean, actual_clean);
        }
    }
}
