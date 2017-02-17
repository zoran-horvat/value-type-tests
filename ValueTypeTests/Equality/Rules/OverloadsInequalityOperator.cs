namespace ValueTypeTests.Equality.Rules
{
    class OverloadsInequalityOperator<T> : ImplementsMethod<T>
    {
        public OverloadsInequalityOperator() : base("op_Inequality", "operator !=", typeof(T), typeof(T))
        {
        }
    }
}
