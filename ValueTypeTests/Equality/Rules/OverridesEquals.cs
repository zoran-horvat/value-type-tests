namespace ValueTypeTests.Equality.Rules
{
    class OverridesEquals<T> : ImplementsMethod<T>
    {
        public OverridesEquals() : base("Equals", typeof(object))
        {
        }
    }
}
