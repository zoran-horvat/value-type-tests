using System.Collections.Generic;

namespace ValueTypeTests.Equality.Rules
{
    interface ITestRule
    {
        IEnumerable<string> GetErrorMessages();
    }
}
