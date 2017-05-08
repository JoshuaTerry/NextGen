using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Helpers
{
    [TestClass]
    public class StringHelperTests
    {
        private const string TESTDESCR = "Shared | Helpers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_CollapseSpaces()
        {
            Assert.AreEqual("ABC DEF", StringHelper.CollapseSpaces("ABC  DEF"));
            Assert.AreEqual("ABC DEF", StringHelper.CollapseSpaces("ABC DEF"));
            Assert.AreEqual(" ABC DEF ", StringHelper.CollapseSpaces("   ABC   DEF   "));
            Assert.IsNull(StringHelper.CollapseSpaces(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_FirstNonBlank()
        {
            Assert.AreEqual("ABC", StringHelper.FirstNonBlank("", "", "", "ABC", "DEF"));
            Assert.AreEqual("ABC", StringHelper.FirstNonBlank(null, null, null, "ABC", "DEF"));
            Assert.AreEqual("", StringHelper.FirstNonBlank("", "", ""));
            Assert.AreEqual("", StringHelper.FirstNonBlank(null, null, null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_IsSameAs()
        {
            Assert.IsTrue(StringHelper.IsSameAs("ABC", "  abc  "));
            Assert.IsTrue(StringHelper.IsSameAs(" AbC ", "aBc"));
            Assert.IsTrue(StringHelper.IsSameAs("", null));
            Assert.IsTrue(StringHelper.IsSameAs(null, ""));

            Assert.IsFalse(StringHelper.IsSameAs("ABC", "ABD"));
            Assert.IsFalse(StringHelper.IsSameAs(" AbC ", "aCb"));
            Assert.IsFalse(StringHelper.IsSameAs("ABC", null));
            Assert.IsFalse(StringHelper.IsSameAs(null, "ABC"));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_LettersAndDigits()
        {
            Assert.AreEqual("A2C", StringHelper.LettersAndDigits("  %$@#!A-\t2?^\\_C+:,*."));
            Assert.AreEqual("", StringHelper.LettersAndDigits("  %$@#!-\t?^\\_+:,*."));
            Assert.AreEqual("", StringHelper.LettersAndDigits(""));
            Assert.AreEqual("", StringHelper.LettersAndDigits(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_FormalCase()
        {
            Assert.AreEqual("Test", StringHelper.FormalCase("TEST"));
            Assert.AreEqual("Test", StringHelper.FormalCase("test"));
            Assert.AreEqual("Test string", StringHelper.FormalCase("TEST STRING"));
            Assert.AreEqual("", StringHelper.FormalCase(""));
            Assert.IsNull(StringHelper.FormalCase(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_SqlLike()
        {
            Assert.IsTrue(StringHelper.SqlLike("ABCDEF", "ABCDEF"));
            Assert.IsTrue(StringHelper.SqlLike("ABCDEF", "A%C%F"));
            Assert.IsTrue(StringHelper.SqlLike("ABCDEF", "AB__EF"));

            Assert.IsFalse(StringHelper.SqlLike("ABCDEF", "ABDCEF"));
            Assert.IsFalse(StringHelper.SqlLike("ABCDEF", "B%"));
            Assert.IsFalse(StringHelper.SqlLike("ABCDEF", "AB_EF"));

            Assert.IsFalse(StringHelper.SqlLike("ABCDEF", ""));
            Assert.IsFalse(StringHelper.SqlLike("ABCDEF", null));
            Assert.IsFalse(StringHelper.SqlLike("", "%"));
            Assert.IsFalse(StringHelper.SqlLike(null, "%"));

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_SplitIntoWords()
        {
            var expected = "ABC DEF XYZ".Split(' ');

            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("ABC DEF XYZ"));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("ABC (DEF) XYZ"));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("ABC-DEF_XYZ"));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("+ABC+ DEF=XYZ="));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("[ABC] {DEF} |XYZ"));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords(@"\ABC/ :DEF; !XYZ?"));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("'ABC\" ,DEF. @XYZ#"));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("$ABC% ^DEF& *XYZ"));

            CollectionAssert.AreNotEquivalent(expected, StringHelper.SplitIntoWords("AbcDefXyz"));
            CollectionAssert.AreNotEquivalent(expected, StringHelper.SplitIntoWords("ABC1DEF2XYZ"));

            expected = new string[0];
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords(""));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords("[ ]"));
            CollectionAssert.AreEquivalent(expected, StringHelper.SplitIntoWords(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_ToPlural()
        {
            Assert.AreEqual("cats", StringHelper.ToPlural("cat"));
            Assert.AreEqual("Black cats", StringHelper.ToPlural("Black cat"));
            Assert.AreEqual(" cats", StringHelper.ToPlural(" cat "));
            Assert.AreEqual("Fairies", StringHelper.ToPlural("Fairy"));
            Assert.AreEqual("moneys", StringHelper.ToPlural("money"));
            Assert.AreEqual("days", StringHelper.ToPlural("day"));
            Assert.AreEqual("boys", StringHelper.ToPlural("boy"));
            Assert.AreEqual("guys", StringHelper.ToPlural("guy"));
            Assert.AreEqual("churches", StringHelper.ToPlural("church"));
            Assert.AreEqual("brushes", StringHelper.ToPlural("brush"));
            Assert.AreEqual("lives", StringHelper.ToPlural("life"));
            Assert.AreEqual("drives", StringHelper.ToPlural("drive"));
            Assert.AreEqual("loaves", StringHelper.ToPlural("loaf"));
            Assert.AreEqual("axes", StringHelper.ToPlural("ax"));
            Assert.AreEqual("fuzzes", StringHelper.ToPlural("fuzz"));
            Assert.AreEqual("leaves", StringHelper.ToPlural("leaf"));
            Assert.AreEqual("shelves", StringHelper.ToPlural("shelf"));
            Assert.AreEqual("bosses", StringHelper.ToPlural("boss"));
            Assert.AreEqual("data", StringHelper.ToPlural("data"));
            Assert.AreEqual("children", StringHelper.ToPlural("child"));
            Assert.AreEqual("teeth", StringHelper.ToPlural("tooth"));
            Assert.AreEqual("men", StringHelper.ToPlural("man"));
            Assert.AreEqual("women", StringHelper.ToPlural("woman"));
            Assert.AreEqual("mice", StringHelper.ToPlural("mouse"));

            Assert.AreEqual("x", StringHelper.ToPlural("x"));
            Assert.AreEqual("", StringHelper.ToPlural(""));
            Assert.IsNull(StringHelper.ToPlural(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_ToSingular()
        {
            Assert.AreEqual("cat", StringHelper.ToSingular("cats"));
            Assert.AreEqual("Black cat", StringHelper.ToSingular("Black cats"));
            Assert.AreEqual(" cat", StringHelper.ToSingular(" cats "));
            Assert.AreEqual("Fairy", StringHelper.ToSingular("Fairies"));
            Assert.AreEqual("money", StringHelper.ToSingular("moneys"));
            Assert.AreEqual("day", StringHelper.ToSingular("days"));
            Assert.AreEqual("boy", StringHelper.ToSingular("boys"));
            Assert.AreEqual("guy", StringHelper.ToSingular("guys"));
            Assert.AreEqual("church", StringHelper.ToSingular("churches"));
            Assert.AreEqual("brush", StringHelper.ToSingular("brushes"));
            Assert.AreEqual("life", StringHelper.ToSingular("lifes"));
            Assert.AreEqual("drive", StringHelper.ToSingular("drives"));
            Assert.AreEqual("loaf", StringHelper.ToSingular("loaves"));
            Assert.AreEqual("ax", StringHelper.ToSingular("axes"));
            Assert.AreEqual("fuzz", StringHelper.ToSingular("fuzzes"));
            Assert.AreEqual("leaf", StringHelper.ToSingular("leaves"));
            Assert.AreEqual("shelf", StringHelper.ToSingular("shelves"));
            Assert.AreEqual("boss", StringHelper.ToSingular("bosses"));
            Assert.AreEqual("data", StringHelper.ToSingular("data"));
            Assert.AreEqual("child", StringHelper.ToSingular("children"));
            Assert.AreEqual("tooth", StringHelper.ToSingular("teeth"));
            Assert.AreEqual("man", StringHelper.ToSingular("men"));
            Assert.AreEqual("woman", StringHelper.ToSingular("women"));
            Assert.AreEqual("person", StringHelper.ToSingular("people"));

            Assert.AreEqual("x", StringHelper.ToSingular("x"));
            Assert.AreEqual("", StringHelper.ToSingular(""));
            Assert.IsNull(StringHelper.ToSingular(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_ToSeparateWords()
        {
            Assert.AreEqual("Test String", StringHelper.ToSeparateWords("testString"));
            Assert.AreEqual("Test String", StringHelper.ToSeparateWords("TestString"));
            Assert.AreEqual("Test String", StringHelper.ToSeparateWords("Test-String"));
            Assert.AreEqual("This ID Value", StringHelper.ToSeparateWords("thisIDValue"));
            Assert.AreEqual("Route 65 South", StringHelper.ToSeparateWords("route65south"));

            Assert.AreEqual("Test", StringHelper.ToSeparateWords("test"));
            Assert.AreEqual("", StringHelper.ToSeparateWords("  "));
            Assert.AreEqual("", StringHelper.ToSeparateWords(""));
            Assert.IsNull(StringHelper.ToSeparateWords(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_Truncate()
        {
            Assert.AreEqual("Test", StringHelper.Truncate("Testing", 4));
            Assert.AreEqual("Testing", StringHelper.Truncate("Testing", 7));
            Assert.AreEqual("Testing", StringHelper.Truncate("Testing", 10));
            Assert.AreEqual("", StringHelper.Truncate("Testing", 0));
            Assert.IsNull(StringHelper.Truncate(null, 10));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_Masked()
        {
            Assert.AreEqual("***456", StringHelper.Masked("123456", 3, '*'));
            Assert.AreEqual("XX3456", StringHelper.Masked("123456", 4, 'X'));
            Assert.AreEqual("*****", StringHelper.Masked("123456", 6, '*'));
            Assert.AreEqual("*****", StringHelper.Masked("123456", 10, '*'));
            Assert.AreEqual("******", StringHelper.Masked("123456", 0, '*'));
            Assert.AreEqual("", StringHelper.Masked("", 5, '*'));
            Assert.IsNull(StringHelper.Masked(null, 5, '*'));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_RemoveAllMacros()
        {
            Assert.AreEqual("ABC", StringHelper.RemoveAllMacros("A{BC}BC"));
            Assert.AreEqual("ABC", StringHelper.RemoveAllMacros("{ }A{BC}B{{}C"));
            Assert.AreEqual("AB}C", StringHelper.RemoveAllMacros("AB{12}}C"));
            Assert.AreEqual("", StringHelper.RemoveAllMacros(""));
            Assert.IsNull(StringHelper.RemoveAllMacros(null));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_SplitDualName()
        {
            string part2;

            Assert.AreEqual("John", StringHelper.SplitDualName("John & Mary", out part2));
            Assert.AreEqual("Mary", part2);

            Assert.AreEqual("John", StringHelper.SplitDualName("John/Mary", out part2));
            Assert.AreEqual("Mary", part2);

            Assert.AreEqual("John Mary", StringHelper.SplitDualName("John Mary", out part2));
            Assert.AreEqual("", part2);

            Assert.AreEqual("", StringHelper.SplitDualName("", out part2));
            Assert.AreEqual("", part2);

            Assert.IsNull(StringHelper.SplitDualName(null, out part2));
            Assert.AreEqual(part2, "");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_GetBestMatch()
        {
            string[] candidates = "Posted Unposted Active Inactive Approved Unapproved".Split(' ');
            Assert.AreEqual("Active", StringHelper.GetBestMatch("active", candidates));
            Assert.AreEqual("Active", StringHelper.GetBestMatch("act", candidates));
            Assert.AreEqual("Inactive", StringHelper.GetBestMatch("INACTIVE X", candidates));
            Assert.AreEqual("Inactive", StringHelper.GetBestMatch("Isactive", candidates));
            Assert.AreEqual("Inactive", StringHelper.GetBestMatch("i", candidates));
            Assert.AreEqual("Inactive", StringHelper.GetBestMatch("innactive", candidates));
            Assert.AreEqual("Inactive", StringHelper.GetBestMatch("inactve", candidates));
            Assert.AreEqual("Active", StringHelper.GetBestMatch("A", candidates));
            Assert.AreEqual("Approved", StringHelper.GetBestMatch("ap", candidates));
            Assert.AreEqual("Active", StringHelper.GetBestMatch("activated", candidates));
            Assert.AreEqual("Approved", StringHelper.GetBestMatch("Proven", candidates));
            Assert.IsNull(StringHelper.GetBestMatch("", candidates));
            Assert.IsNull(StringHelper.GetBestMatch(null, candidates));
            Assert.IsNull(StringHelper.GetBestMatch("Active"));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_LevenshteinDistance()
        {
            Assert.AreEqual(0, StringHelper.LevenshteinDistance("kitten", "kitten")); // match
            Assert.AreEqual(1, StringHelper.LevenshteinDistance("kitten", "kiten")); // deletion
            Assert.AreEqual(1, StringHelper.LevenshteinDistance("kitten", "mitten")); // subsitution
            Assert.AreEqual(1, StringHelper.LevenshteinDistance("kitten", "kittens")); // insertion
            Assert.AreEqual(3, StringHelper.LevenshteinDistance("kitten", "sitting")); // subsitutions + insertion
            Assert.AreEqual(2, StringHelper.LevenshteinDistance("Kitten", "kitteN")); // case sensitive
            Assert.AreEqual(6, StringHelper.LevenshteinDistance("kitten", "")); // max length
            Assert.AreEqual(6, StringHelper.LevenshteinDistance("", "kitten")); 
            Assert.AreEqual(6, StringHelper.LevenshteinDistance("kitten", null));
            Assert.AreEqual(6, StringHelper.LevenshteinDistance(null, "kitten"));
            Assert.AreEqual(0, StringHelper.LevenshteinDistance(null, "")); // null cases
            Assert.AreEqual(0, StringHelper.LevenshteinDistance("", null)); 
            Assert.AreEqual(0, StringHelper.LevenshteinDistance(null, null));
            Assert.AreEqual(0, StringHelper.LevenshteinDistance("", "")); 
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void StringHelper_Min3()
        {
            Assert.AreEqual(5, StringHelper.Min3(7, 5, 6));
            Assert.AreEqual(5, StringHelper.Min3(5, 6, 7));
            Assert.AreEqual(5, StringHelper.Min3(7, 6, 5));
        }
    }
}