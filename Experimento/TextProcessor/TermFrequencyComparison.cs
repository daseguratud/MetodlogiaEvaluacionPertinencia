using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessorTools
{
    public class TermFrequencyComparison
    {
        public string Term { get; set; }
        public float ReferenceFrequency { get; set; }
        public float TargetFrequency { get; set; }
        public TermFrequencyComparison(string term, int referenceFrequency, int targetFrequency)
        {
            Term = term;
            ReferenceFrequency = referenceFrequency;
            TargetFrequency = targetFrequency;
        }
        public TermFrequencyComparison() { }
    }
}
