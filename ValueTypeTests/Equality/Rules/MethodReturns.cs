using System;
using System.Collections.Generic;
using System.Reflection;

namespace ValueTypeTests.Equality.Rules
{
    class MethodReturns<T, TResult> : ITestRule
    {
        private MethodInfo Method { get; }
        private string MethodLabel { get; }
        private T TargetObject { get; }
        private TResult ExpectedResult { get; }
        private object[] MethodParameters { get; }
        private string TestCase { get; }

        private MethodReturns(MethodInfo method, string methodLabel, T target, TResult result, string testCase, params object[] parameters)
        {
            this.Method = method;
            this.MethodLabel = methodLabel;
            this.TargetObject = target;
            this.ExpectedResult = result;
            this.MethodParameters = parameters;
            this.TestCase = testCase;
        }

        public static MethodReturns<T, TResult> InstanceMethod(
            MethodInfo method, T target, TResult result,
            string testCase, params object[] parameters) =>
            new MethodReturns<T, TResult>(method, method.Name, target, result, testCase, parameters);

        public static MethodReturns<T, TResult> Operator(
            MethodInfo method, string operatorLabel, T obj1, T obj2, TResult result, string testCase) =>
            new MethodReturns<T, TResult>(method, operatorLabel, default(T), result, testCase, obj1, obj2);

        public IEnumerable<string> GetErrorMessages()
        {
            IList<string> errors = new List<string>();

            try
            {
                TResult actualResult = (TResult)this.Method.Invoke(this.TargetObject, this.MethodParameters);
                if (!actualResult.Equals(this.ExpectedResult))
                    errors.Add(
                        $"{MethodExtensions.GetSignature(this.Method, this.MethodLabel)} returned {actualResult} when expecting {this.ExpectedResult} - {this.TestCase}");
            }
            catch (TargetInvocationException invocationExc)
            {
                errors.Add($"{MethodExtensions.GetSignature(this.Method)} failed - {invocationExc.Message}.");
            }
            catch (Exception exc)
            {
                errors.Add($"{MethodExtensions.GetSignature(this.Method)} failed - {exc.Message}.");
            }

            return errors;
        }
    }
}
