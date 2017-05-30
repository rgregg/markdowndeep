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
    class OrderedListTests
    {


        public static IEnumerable<TestCaseData> GetTests()
        {
            return Utils.GetTests("orderedlist");
        }

        [Test, TestCaseSource("GetTests")]
        public void Test_orderedlists(string resourceName)
        {
            Utils.RunResourceTest(resourceName);
        }
    }
}
