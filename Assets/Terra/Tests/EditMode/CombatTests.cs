using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace Terra.Tests
{
    public sealed class CombatTests
    {
        public static IEnumerable<TestCaseData> Specifications()
        {
            foreach (var method in typeof(CombatSpecifications).GetMethods(BindingFlags.Public | BindingFlags.Static))
                yield return new TestCaseData(method).SetName(method.Name);
        }
        [TestCaseSource(nameof(Specifications))]
        public void GameplaySpecification(MethodInfo method)
        {
            try { method.Invoke(null, null); }
            catch (TargetInvocationException error) { throw error.InnerException ?? error; }
        }
    }
}
