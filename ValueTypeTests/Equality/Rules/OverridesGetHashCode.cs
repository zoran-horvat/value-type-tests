namespace ValueTypeTests.Equality.Rules
{
    class OverridesGetHashCode<T> : ImplementsMethod<T>
    {
        public OverridesGetHashCode() : base("GetHashCode")
        {
        }
    }
}
