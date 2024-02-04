using BpCop.Rules;
using BpCop.Rules.TestRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BpCop.RulesTests
{
    [TestClass()]
    public class RuleTests
    {
        private static Engine TestEngine;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestEngine = new Engine(new[]
            {
                Path.Combine(context.DeploymentDirectory,@"..\..\..\..",@"BpCop.Rules.Design\bin\Debug\netstandard2.0"),
                Path.Combine(context.DeploymentDirectory,@"..\..\..\..",@"BpCop.Rules.Security\bin\Debug\netstandard2.0"),
                Path.Combine(context.DeploymentDirectory,@"..\..\..\..",@"BpCop.Rules.Programming\bin\Debug\netstandard2.0")
            });
        }

        public static IEnumerable<object[]> TestData { get; } = new[]
        {
            new object[] { "DS001", "DS001 - Test", "DS001 Negative", 1 },
            ["DS001", "DS001 - Test", "DS001 Positive", 0],
            ["DS002", "DS002 - Test", "Main Page", 1],
            ["DS002", "DS002 - Tests", "Initialise", 0],
            ["DS003", "DS003 - Test", "Negative", 1],
            ["DS003", "DS003 - Test", "Positive", 0],
            ["DS004", "DS004 - negative Test", "Main Page", 1],
            ["DS004", "DS004 - positive Test", "Main Page", 0],
            ["DS005", "DS005 - negative Test", "Main Page", 1],
            ["DS005", "DS005 - positive Test", "Main Page", 0],
            ["DS006", "DS006 - Test", "Negative", 1],
            ["DS006", "DS006 - Test", "Positive", 0],
            ["DS007", "DS007 - Test", "Negative", 1],
            ["DS007", "DS007 - Test", "Positive", 0],
            ["DS008", "DS008 - negative Test", "", 1],
            ["DS008", "DS008 - positive Test", "", 0],
            ["DS009", "DS009 - Test", "Negative", 1],
            ["DS009", "DS009 - Test", "Main Page", 0],
            ["DS010", "DS010 - Test", "Negative", 1],
            ["DS010", "DS010 - Test", "Positive 1", 0],
            ["DS010", "DS010 - Test", "Positive 2", 0],
            ["DS011", "DS011 - Test", "Negative", 1],
            ["DS011", "DS011 - Test", "Positive", 0],
            ["DS012", "DS012 - Test", "Negative", 1],
            ["DS012", "DS012 - Test", "Positive", 0],
            ["DS013", "DS013 - Test", "Action 1", 1],
            ["DS014", "DS014 - Test", "Main Page", 1],
            ["DS015", "DS015 - Test", "Main Page", 2],
            ["DS016", "DS016 - Test", "Main Page", 1],
            ["DS017", "DS017 - Test", "Main Page", 1],
            ["DS018", "DS018 - Test", "Main Page", 1],
            ["DS018", "DS018 - VBO Tests", "Param in Parameter", 0],
            ["DS019", "DS019 - Test", "Main Page", 1],
            ["DS020", "DS020 - Test", "Main Page", 1],
            ["DS021", "DS021 - Test", "", 1],
            ["DS022", "DS022 - Test", "Never Called Page", 1],
            ["DS022", "DS022 - Test", "Called Page", 0],
            ["DS023", "DS023 - Test", "Never Called Page", 1],
            ["DS023", "DS023 - Test", "Called Page", 0],
            ["DS024", "DS024 - Test", "Main Page", 1],
            ["DS025", "DS025 - Test", "", 2],
            ["DS026", "DS026 - Test", "", 1],
            ["DS027", "DS027 - Test", "Private", 1],
            ["DS028", "DS028 - Test", "Negative", 1],
            ["DS029", "DS029 - Test", "Main Page", 1],
            ["DS029", "DS029 - Test", "Positive 1", 0],
            ["DS029", "DS029 - Test", "Positive 2", 0],
            ["DS030", "DS030 - Test", "Main Page", 1],

            ["SC001", "SC001 - Test", "Main Page", 1],
            ["SC002", "SC002 - Test", "Main Page", 1],
            ["SC002", "SC002 - Test", "Collection Test", 1],
            ["SC003", "SC003 - Test", "Main Page", 1],
            ["SC003", "SC003 - Test", "Alert Test", 1],
            ["SC004", "SC004 - Test", "Page With Secret", 1],
            ["SC005", "SC005 - Test", "Page With Secret", 1],
            ["SC008", "SC008 - Test", "Main Page", 1],

            ["PG001", "PG001 - Test", "", 1],
            ["PG001", "PG001 - positive Test", "", 0],
            ["PG002", "PG002 - Test", "Attach 1", 1],
            ["PG002", "PG002 - Test", "Attach 2", 0],
            ["PG003", "PG003 - Test", "Main Page", 2],
            ["PG004", "PG004 - Test", "Main Page", 1],
            ["PG005", "PG005 - Test", "Negative", 1],
            ["PG005", "PG005 - Test", "Positive", 0],
            ["PG006", "PG006 - Test", "Main Page", 1],
            ["PG007", "PG007 - Test", "Main Page", 1],
            ["PG008", "PG008 - Test", "Main Page", 1],
            ["PG009", "PG009 - Test", "Main Page", 1],
            ["PG010", "PG010 - Test", "Main Page", 1],
            ["PG011", "PG011 - Test", "Main Page", 1],
            ["PG012", "PG012 - Test", "Page 1", 1],
            ["PG012", "PG012 - Test", "Page 2", 0],
            ["PG013", "PG013 - Test", "Negative", 1],
            ["PG013", "PG013 - Test", "Positive", 0],
            ["PG014", "PG014 - Test", "Action 1", 8],
            ["PG015", "PG015 - Test", "Main Page", 3],
            ["PG016", "PG016 - Test", "Decision", 1],
            ["PG016", "PG016 - Test", "Calc", 1],
        };

        [DataTestMethod]
        [DynamicData(nameof(TestData), DynamicDataSourceType.Property)]
        public void FromDatabaseTest(string ruleName, string assetName, string sheetName, int assertCount)
        {
            var processxml = ProcessCenter.LoadAsset(assetName);
            var result = TestEngine.CheckRules(processxml, Guid.Empty, true, new[] { ruleName }, null).Findings
                .Where(r => r.Rule == ruleName && r.Page == sheetName);
            Assert.AreEqual(assertCount, result.Count());
        }

    }
}

