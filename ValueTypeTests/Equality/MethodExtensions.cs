using System.Linq;
using System.Reflection;

namespace ValueTypeTests.Equality
{
    static class MethodExtensions
    {
        public static string GetSignature(this MethodInfo method) =>
            GetSignature(method, method.Name);

        public static string GetSignature(this MethodInfo method, string methodLabel) =>
            $"{methodLabel}({GetArgumentsListSignature(method)})";

        private static string GetArgumentsListSignature(MethodInfo method) =>
            string.Join(", ", GetArgumentSignatures(method));

        private static string[] GetArgumentSignatures(MethodInfo method) =>
            method.GetParameters()
                .Select(param => $"{param.ParameterType.Name}")
                .ToArray();
    }
}
