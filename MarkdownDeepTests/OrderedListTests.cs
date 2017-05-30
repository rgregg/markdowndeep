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

        [Test]
        public void ResumeAfterBreakInOrderedList()
        {
            string input = TestCases.breaklist_md;
            string expected = TestCases.breaklist_html;

            RunTest(input, expected);
        }

        [Test]
        public void StartNewListAfterBreakInOrderedList()
        {
            string input = TestCases.donotbreaklist_md;
            string expected = TestCases.donotbreaklist_html;

            RunTest(input, expected);
        }

        [Test]
        public void StartListWith10()
        {
            string input = TestCases.startwith10_md;
            string expected = TestCases.startwith10_html;

            RunTest(input, expected);
        }


        private void RunTest(string input, string expected)
        {
            var md = new MarkdownDeep.Markdown();
            md.RespectOrderedListStartValues = true;

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
