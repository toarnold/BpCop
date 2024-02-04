using BpCop.Rules;
using BpCop.Rules.TestRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BpCop.RulesTests
{
    [TestClass()]
    public class JustificationTests
    {
        private static Engine TestEngine;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestEngine = new Engine(new[]
            {
                Path.Combine(context.DeploymentDirectory,@"..\..\..\..",@"BpCop.Rules.Design\bin\Debug\netstandard2.0"),
            });
        }

        [TestMethod]
        public void GlobalJustificationTest()
        {
            var processxml = ProcessCenter.LoadAsset("Global Justification - Test");
            var result = TestEngine
                .CheckRules(processxml, Guid.Empty, true, new[] { "DS003" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should work
            Assert.AreEqual(0, result.Count(), "Justification working");

            // Remove justification text
            var xdoc = XDocument.Parse(processxml);
            var element = xdoc.XPathSelectElement("//stage[@type='Note']/narrative");
            element.Value = string.Empty;

            // Do it again
            result = TestEngine
                .CheckRules(xdoc.ToString(), Guid.Empty, true, new[] { "DS003" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should removed now
            Assert.AreEqual(1, result.Count(), "Justification removed");
        }

        [TestMethod]
        public void PageJustificationTest()
        {
            var processxml = ProcessCenter.LoadAsset("Page Justification - Test");
            var result = TestEngine
                .CheckRules(processxml, Guid.Empty, true, new[] { "DS003" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should work
            Assert.AreEqual(0, result.Count(), "Justification working");

            // Remove justification text
            var xdoc = XDocument.Parse(processxml);
            var element = xdoc.XPathSelectElement("//stage[@type='Note']/narrative");
            element.Value = string.Empty;

            // Do it again
            result = TestEngine
                .CheckRules(xdoc.ToString(), Guid.Empty, true, new[] { "DS003" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should removed now
            Assert.AreEqual(1, result.Count(), "Justification removed");
        }

        [TestMethod]
        public void StageJustificationTest()
        {
            var processxml = ProcessCenter.LoadAsset("Stage Justification - Test");
            var result = TestEngine
                .CheckRules(processxml, Guid.Empty, true, new[] { "DS009" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should work
            Assert.AreEqual(0, result.Count(), "Justification working");

            // Remove justification text
            var xdoc = XDocument.Parse(processxml);
            var element = xdoc.XPathSelectElement("//stage[@type='Note']/narrative");
            element.Value = string.Empty;

            // Do it again
            result = TestEngine
                .CheckRules(xdoc.ToString(), Guid.Empty, true, new[] { "DS009" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should removed now
            Assert.AreEqual(1, result.Count(), "Justification removed");
        }

        [TestMethod]
        public void AppModelJustificationTest()
        {
            var processxml = ProcessCenter.LoadAsset("AppModel Justification - Test");
            var result = TestEngine
                .CheckRules(processxml, Guid.Empty, true, new[] { "DS025" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should work
            Assert.AreEqual(1, result.Count(), "Justification working");

            // Remove justification text
            var xdoc = XDocument.Parse(processxml);
            var element = xdoc.XPathSelectElement("//element[@name='Main Window']/description");
            element.Value = string.Empty;

            // Do it again
            result = TestEngine
                .CheckRules(xdoc.ToString(), Guid.Empty, true, new[] { "DS025" }, null)
                .Findings
                .Where(r => string.IsNullOrEmpty(r.JustificationLevel)); // Select non justified only

            // Justification should removed now
            Assert.AreEqual(2, result.Count(), "Justification removed");
        }

    }
}
