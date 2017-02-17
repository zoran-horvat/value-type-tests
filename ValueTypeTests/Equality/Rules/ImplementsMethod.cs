using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ValueTypeTests.Equality.Rules
{
    abstract class ImplementsMethod<T> : ITestRule
    {
        private string MethodName { get; }
        private string MethodLabel { get; }
        private Type[] ArgumentTypes { get; }

        private IEnumerable<MethodInfo> targetMethod;
        private Action DiscoverTargetMehtod { get; set; }

        protected ImplementsMethod(string methodName, params Type[] argumentTypes)
            : this(methodName, methodName, argumentTypes)
        {
        }

        protected ImplementsMethod(string methodName, string methodLabel, params Type[] argumentTypes)
        {
            this.MethodName = methodName;
            this.MethodLabel = methodLabel;
            this.ArgumentTypes = argumentTypes;

            this.DiscoverTargetMehtod =  () =>
            {
                MethodInfo method = typeof(T).GetMethod(this.MethodName, this.ArgumentTypes);
                if (method == null)
                    this.targetMethod = Enumerable.Empty<MethodInfo>();
                else
                    this.targetMethod = new[] { method };

                this.DiscoverTargetMehtod = () => { };
            };
        }

        public IEnumerable<MethodInfo> TryGetTargetMethod()
        {
            this.DiscoverTargetMehtod();
            return this.targetMethod;
        }

        public IEnumerable<string> GetErrorMessages()
        {
            if (this.TryGetTargetMethod().All(method => method.DeclaringType != typeof(T)))
                yield return $"{typeof(T).Name} should {this.OverrideLabel} {this.MethodSignature}).";
        }

        private string OverrideLabel
        {
            get
            {
                if (this.TryGetTargetMethod().Any(m => m.GetBaseDefinition() != null))
                    return "override";
                return "overload";
            }
        }
        private string MethodSignature =>
            $"{this.MethodLabel}({string.Join(", ", this.ArgumentTypes.Select(type => type.Name))}";
    }
}
