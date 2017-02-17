using System;
using System.Collections.Generic;
using System.Linq;

namespace ValueTypeTests.Equality
{
    public class EqualityTester<T>
    {
        private T TargetObject { get; }
        private List<Rules.ITestRule> Rules { get; } = new List<Rules.ITestRule>();
        private TypeAnalysis<T> Analysis { get; }

        public EqualityTester(T targetObject)
        {
            this.TargetObject = targetObject;
            this.Analysis = TypeAnalysis<T>.Analyze();

            this.Rules.AddRange(this.Analysis.TypeLevelRules);

            if (!typeof(T).IsValueType)
            {
                this.Rules.AddRange(this.Analysis.GetNotEqualRules(TargetObject, default(T), "inequality to null"));
                this.Rules.AddRange(this.Analysis.GetEqualityOfTwoNulls());
            }
        }

        public EqualityTester<T> EqualTo(T obj)
        {
            this.Rules.AddRange(this.Analysis.GetEqualToRules(this.TargetObject, obj));
            return this;
        }

        public EqualityTester<T> NotEqualTo(T obj, string testCase)
        {
            this.Rules.AddRange(this.Analysis.GetNotEqualRules(this.TargetObject, obj, testCase));
            return this;
        }

        public void Assert()
        {
            List<string> errorMessages = 
                this.Rules.SelectMany(rule => rule.GetErrorMessages()).ToList();

            if (errorMessages.Any())
            {
                string message = 
                    "There were errors testing equality logic:\n" +
                    string.Join(Environment.NewLine, errorMessages.ToArray());

                throw new ValueSemanticException(message);
            }
        }
    }
}
