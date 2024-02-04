using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Rules.Dto;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Dynamic;
using System.Linq;

namespace BpCop.Rules
{
    internal sealed record RuleRuntimeInfo(ICheckRule Rule, ICheckRuleMetadata Metadata);

    public class Engine : IDisposable
    {
        private readonly CompositionContainer _container;
#pragma warning disable IDE0044 // Add readonly modifier: field is managed by MEF
#pragma warning disable 0649 // field is managed by MEF
        [ImportMany]
        private IEnumerable<Lazy<ICheckRule, ICheckRuleMetadata>> _rules;
        private bool disposedValue;
#pragma warning restore 0649
#pragma warning restore IDE0044

        private readonly Dictionary<string, RuleRuntimeInfo> _rulesDictionary;

        private List<string> WhitelistSafe(IEnumerable<string> whitelist)
        {
            // Whitelist empty? Use default list
            if (whitelist is null || !whitelist.Any())
            {
                whitelist = _rulesDictionary.Keys;
            }
            return whitelist.Select(name => name.ToUpperInvariant()).ToList();
        }

        private static List<string> BlacklistSafe(IEnumerable<string> blacklist)
        {
            return (blacklist ?? Enumerable.Empty<string>()).Select(name => name.ToUpperInvariant()).ToList();
        }

#pragma warning disable CS8618 // Consider declaring as nullable. Field _rules is managed by MEF
        public Engine(IEnumerable<string> extensionPaths)
#pragma warning restore CS8618
        {
            // An aggregate catalog that combines multiple catalogs.
            using var initialcatalog = new AggregateCatalog();
            var catalog = extensionPaths.Aggregate(initialcatalog, (c, path) => { c.Catalogs.Add(new DirectoryCatalog(path)); return c; });

            // Create the CompositionContainer with the parts in the catalog.
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);

            // Speed up rules:
            _rulesDictionary = _rules.ToDictionary(r => r.Metadata.RuleName, v => new RuleRuntimeInfo(Rule: v.Value, Metadata: v.Metadata));
        }

        private Justification? GetTopmostJustification(BpProcess process, string ruleName, string pageName, string stageNameOrPath)
        {
            process.Justifications.TryGetValue(ruleName, out var justifications);
            var rule = _rulesDictionary[ruleName].Metadata;

            // Search the first matching justification with highest priority
            var justification = justifications?.FirstOrDefault(j =>
                (rule.JustificationLevels.HasFlag(JustificationLevel.Global) && j.Level == JustificationLevel.Global)
                || (rule.JustificationLevels.HasFlag(JustificationLevel.Page) && j.Level == JustificationLevel.Page && j.PageName == pageName)
                || (rule.JustificationLevels.HasFlag(JustificationLevel.Stage) && j.Level == JustificationLevel.Stage && j.PageName == pageName && j.StageName == stageNameOrPath)
                || (rule.JustificationLevels.HasFlag(JustificationLevel.AppModel) && j.Level == JustificationLevel.AppModel && j.StageName == stageNameOrPath)
            );
            if (justification is not null)
            {
                // Increase usage count
                ++justification.UsageCount;
            }
            return justification;
        }

        public AnalysisInformation CheckRules(string processXml, Guid? processId, bool isPublished, IEnumerable<string> whitelist, IEnumerable<string> blacklist, string customParameter = "{}")
        {
            var process = new BpProcess(processXml, processId, isPublished);
            var bll = BlacklistSafe(blacklist);
            var wtl = WhitelistSafe(whitelist);
            var cp = JObject.Parse(customParameter);
            var findings = wtl
                .Where(w => !bll.Contains(w)) // Skip black listed rules
                .Select(GetRule)
                .Where(w => w is not null) // Skip not found rules
                .SelectMany(rule => CheckRule(process, rule!, cp), (rule, result) => new { Result = result, Justification = GetTopmostJustification(process, rule!.Metadata.RuleName, result.PageName, result.StageName) })
                .Select(s =>
                    new FindingInformation
                    {
                        Rule = s.Result.RuleName,
                        Asset = s.Result.AssetName,
                        AssetType = s.Result.AssetType,
                        Page = s.Result.PageName,
                        Stage = s.Result.StageName,
                        Message = s.Result.FormattedMessage,
                        JustificationLevel = s.Justification?.Level.ToString() ?? string.Empty,
                        Justification = s.Justification?.Message ?? string.Empty
                    }
                )
                .ToList(); // Terminate linq to execute justification usage count 
            var justifications = process
                .Justifications
                .SelectMany(m => m.Value)
                .Where(w => wtl.Contains(w.RuleName) && !bll.Contains(w.RuleName))
                .Select(s => new JustificationInformation
                {
                    Level = s.Level.ToString(),
                    Rule = s.RuleName,
                    Asset = s.AssetName,
                    AssetType = s.AssetType,
                    Page = s.PageName,
                    Stage = s.StageName,
                    Message = s.Message,
                    Used = s.UsageCount > 0
                })
                .ToList();
            return new AnalysisInformation(findings, justifications, GetRuleInfo().Where(info => wtl.Contains(info.Rule)).ToList());
        }

        private static IEnumerable<CheckResult> CheckRule(BpProcess process, RuleRuntimeInfo rule2execute, dynamic customParameter)
        {
            // Check if rules should applied to this type
            if (rule2execute.Metadata.AppliesTo.HasFlag(process.AssetType))
            {
                // build rule parameters
                dynamic parameter = new ExpandoObject();
                if (customParameter[rule2execute.Metadata.RuleName] is not null || !string.IsNullOrEmpty(rule2execute.Metadata.DefaultParameter))
                {
                    parameter = customParameter[rule2execute.Metadata.RuleName] ?? JObject.Parse(rule2execute.Metadata.DefaultParameter);
                }

                var data = new CheckRuleData(process, rule2execute.Metadata, parameter);

                // Execute rule
                return rule2execute.Rule.CheckRule(data);
            }
            else
            {
                return Enumerable.Empty<CheckResult>();
            }
        }

        private IEnumerable<RuleInformation> GetRuleInfo()
        {
            return _rules
                .Select(r => r.Metadata)
                .OrderBy(o => o.RuleName)
                .Select(s => new RuleInformation
                {
                    Set = s.RuleSet,
                    Rule = s.RuleName,
                    Target = s.AppliesTo.ToString(),
                    JustificationLevels = s.JustificationLevels.ToString(),
                    Description = s.Description,
                    Parameter = s.DefaultParameter
                });
        }

        public IList<RuleInformation> GetRules() => GetRuleInfo().ToList();

        private RuleRuntimeInfo? GetRule(string ruleName)
        {
            _rulesDictionary.TryGetValue(ruleName.ToUpperInvariant(), out var rule);
            return rule is not null ? new RuleRuntimeInfo(Rule: rule.Rule, Metadata: rule.Metadata): null;
        }

        public bool HasRule(string ruleName) => ruleName is not null && _rulesDictionary.ContainsKey(ruleName.ToUpperInvariant());

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _container.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
