using System;
using System.Reflection;
using Terra.Tests;

internal static class Program
{
    private static int Main()
    {
        int failures = 0, total = 0;
        foreach (var method in typeof(CombatSpecifications).GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            total++;
            try { method.Invoke(null, null); Console.WriteLine("PASS " + method.Name); }
            catch (Exception error) { failures++; Console.WriteLine("FAIL " + method.Name + ": " + (error.InnerException ?? error).Message); }
        }
        Console.WriteLine($"{total - failures}/{total} gameplay specifications passed. Unity runtime not exercised.");
        return failures == 0 ? 0 : 1;
    }
}
