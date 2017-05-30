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

    }
}
