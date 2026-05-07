using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessorTools
{
    public class NormalizationRule
    {
        public string CanonicalForm { get; private set; }
        public List<string> LexicalVariants {  get; private set; }
        public NormalizationRule(string CanonicalForm)
        {
            this.CanonicalForm = CanonicalForm;
            LexicalVariants = new List<string>();
        }
        public static List<NormalizationRule> LoadRulesFromFile(string FileName) 
        { 
            var lines = File.ReadAllLines(FileName);
            return RulesFromLines(lines);
        }
        private static List<NormalizationRule> RulesFromLines(string[] lines)
        {
            var rules = new List<NormalizationRule>();
            foreach (var line in lines)
            {
                if (line[0] == '#') continue;
                var parts = line.Split("<-");
                if (parts.Length != 2) continue;
                var canonicalForm = parts[0].Trim();
                var variants = parts[1].Split(';').Select(v => v.Trim()).ToList();
                var rule = new NormalizationRule(canonicalForm);
                rule.LexicalVariants = variants;
                rules.Add(rule);
            }
            return rules;
        }
    }
}
