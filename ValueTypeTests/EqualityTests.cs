using ValueTypeTests.Equality;

namespace ValueTypeTests
{
    public static class EqualityTests
    {
        public static EqualityTester<T> For<T>(T obj) =>
            new EqualityTester<T>(obj);
    }
}