using System;
using System.Collections.Generic;
using System.Reflection;

namespace ValueTypeTests.Equality.Rules
{
    class GetHashCodeEqualReturns<T> : ITestRule
    {
        private MethodInfo Method { get; }
        private T TargetInstance { get; }
        private T OtherObject { get; }

        public GetHashCodeEqualReturns(MethodInfo method, T obj1, T obj2)
        {
            this.Method = method;
            this.TargetInstance = obj1;
            this.OtherObject = obj2;
        }

        public IEnumerable<string> GetErrorMessages()
        {
            IList<string> errors = new List<string>();

            try
            {
                int hash1 = (int)this.Method.Invoke(this.TargetInstance, new object[0]);
                int hash2 = (int) this.Method.Invoke(this.OtherObject, new object[0]);

                if (hash1 != hash2)
                    errors.Add($"{MethodExtensions.GetSignature(this.Method)} returned distinct values on equal objects.");
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
