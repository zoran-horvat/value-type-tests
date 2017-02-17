using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ValueTypeTests.Equality.Rules;

namespace ValueTypeTests.Equality
{
    class TypeAnalysis<T>
    {
        public IEnumerable<Rules.ITestRule> TypeLevelRules => this.Rules;
        private IList<Rules.ITestRule> Rules { get; } = new List<Rules.ITestRule>();

        private IEnumerable<MethodInfo> EqualsMethod { get; set; }
        private IEnumerable<MethodInfo> StrongEqualsMethod { get; set; }
        private IEnumerable<MethodInfo> GetHashCodeMethod { get; set; }
        private IEnumerable<MethodInfo> EqualityOperator { get; set; }
        private IEnumerable<MethodInfo> InequalityOperator { get; set; }

        public static TypeAnalysis<T> Analyze()
        {
            TypeAnalysis<T> analysis = new TypeAnalysis<T>();

            AnalyzeEquals(analysis);
            AnalyzeGetHashCode(analysis);
            AnalyzeEqualityOperator(analysis);
            AnalyzeInequalityOperator(analysis);
            AnalyzeEquatable(analysis);
            AnalyzeSealed(analysis);

            return analysis;
        }

        private static void AnalyzeEquals(TypeAnalysis<T> analysis)
        {
            Rules.ImplementsMethod<T> rule = new Rules.OverridesEquals<T>();
            analysis.EqualsMethod = rule.TryGetTargetMethod();
            analysis.Rules.Add(rule);
        }

        private static void AnalyzeGetHashCode(TypeAnalysis<T> analysis)
        {
            Rules.ImplementsMethod<T> rule = new Rules.OverridesGetHashCode<T>();
            analysis.GetHashCodeMethod = rule.TryGetTargetMethod();
            analysis.Rules.Add(rule);
        }

        private static void AnalyzeEqualityOperator(TypeAnalysis<T> analysis)
        {
            Rules.ImplementsMethod<T> rule = new Rules.OverloadsEqualityOperator<T>();
            analysis.EqualityOperator = rule.TryGetTargetMethod();
            analysis.Rules.Add(rule);
        }

        private static void AnalyzeInequalityOperator(TypeAnalysis<T> analysis)
        {
            Rules.ImplementsMethod<T> rule = new Rules.OverloadsInequalityOperator<T>();
            analysis.InequalityOperator = rule.TryGetTargetMethod();
            analysis.Rules.Add(rule);
        }

        private static void AnalyzeEquatable(TypeAnalysis<T> analysis)
        {
            Rules.ImplementsIEquatable<T> rule = new Rules.ImplementsIEquatable<T>();
            analysis.StrongEqualsMethod = rule.TryGetTargetMethod();
            analysis.Rules.Add(rule);
        }

        private static void AnalyzeSealed(TypeAnalysis<T> analysis)
        {
            analysis.Rules.Add(new Rules.IsTypeSealed<T>());
        }

        public IEnumerable<Rules.ITestRule> GetEqualToRules(T instance, T other)
        {
            string testCase = "equal objects";
            return
                this.GetEqualsReturns(instance, other, true, testCase)
                    .Concat(this.GetEqualityOperatorReturns(instance, other, true, testCase))
                    .Concat(this.GetInequalityOperatorReturns(instance, other, false, testCase))
                    .Concat(this.GetStrongEqualsReturns(instance, other, true, testCase))
                    .Concat(this.GetEqualGetHashCodeReturns(instance, other))
                    .ToList();
        }

        public IEnumerable<Rules.ITestRule> GetNotEqualRules(T instance, T other, string testCase)
        {
            return
                this.GetEqualsReturns(instance, other, false, testCase)
                    .Concat(this.GetEqualityOperatorReturns(instance, other, false, testCase))
                    .Concat(this.GetInequalityOperatorReturns(instance, other, true, testCase))
                    .Concat(this.GetStrongEqualsReturns(instance, other, false, testCase))
                    .ToList();
        }

        public IEnumerable<Rules.ITestRule> GetEqualityOfTwoNulls()
        {
            return
                this.GetEqualityOperatorReturns(default(T), default(T), true, "equality of two nulls")
                    .Concat(this.GetInequalityOperatorReturns(default(T), default(T), false, "equality of two nulls"))
                    .ToList();
        }

        private IEnumerable<Rules.ITestRule> GetEqualsReturns(T instance, T other, bool result, string testCase) =>
            this.EqualsMethod.Select<MethodInfo, Rules.MethodReturns<T, bool>>(
                method => MethodReturns<T, bool>.InstanceMethod(method, instance, result, testCase, other));

        private IEnumerable<Rules.ITestRule> GetEqualityOperatorReturns(T instance, T other, bool result, string testCase) =>
            this.EqualityOperator.Select<MethodInfo, Rules.MethodReturns<T, bool>>(
                method => MethodReturns<T, bool>.Operator(method, "operator ==", instance, other, result, testCase));

        private IEnumerable<Rules.ITestRule> GetInequalityOperatorReturns(T instance, T other, bool result, string testCase) =>
            this.InequalityOperator.Select<MethodInfo, Rules.MethodReturns<T, bool>>(
                method => MethodReturns<T, bool>.Operator(method, "operator !=", instance, other, result, testCase));

        private IEnumerable<Rules.ITestRule> GetStrongEqualsReturns(T instance, T other, bool result, string testCase) =>
            this.StrongEqualsMethod.Select<MethodInfo, Rules.MethodReturns<T, bool>>(
                method => MethodReturns<T, bool>.InstanceMethod(method, instance, result, testCase, other));

        private IEnumerable<Rules.ITestRule> GetEqualGetHashCodeReturns(T instance, T other) =>
            this.GetHashCodeMethod.Select(
                method => new Rules.GetHashCodeEqualReturns<T>(method, instance, other));
    }
}
