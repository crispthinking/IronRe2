using System;
using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;
using Xunit;
using Xunit.Abstractions;

// requires the dotMemory Unit package

namespace IronRe2.Tests;

// Allow tests to run even if dotMemory isn't supported on the current platform.
public class MemoryLeakTests
{
    public MemoryLeakTests(ITestOutputHelper output)
    {
        // Link dotMemory Unit output to xUnit's ITestOutputHelper
        DotMemoryUnitTestOutput.SetOutputMethod(output.WriteLine);
    }

    /// <summary>
    ///     This test creates many Regex and RegexSet objects.
    ///     It first captures a memory snapshot, then runs the workload,
    ///     and finally compares the difference to assert that no additional instances remain.
    /// </summary>
    [Fact]
    [DotMemoryUnit(FailIfRunWithoutSupport = false)]
    public void MemoryLeakTestWithDotMemoryUnit_UsingSnapshot()
    {
        if (!dotMemoryApi.IsEnabled)
        {
            return;
        }

        // Capture an initial snapshot before running the workload.
        var snapshot = dotMemory.Check();

        // Simulate workload by creating and disposing objects.
        for (var i = 0; i < 1000; i++)
        {
            // Create a Regex instance, use it, and dispose.
            using (Regex regex = new(@"\w+"))
            {
                regex.IsMatch("test string");
            }

            // Create a RegexSet instance and use it.
            string[] patterns = [@"\d+", @"\w+", "[a-z]+", "[A-Z]+", @"\s+", @"\W+", @"\D+"];
            RegexSet set = new(patterns);
            set.Match("123 abc");

            // Test with invalid regex patterns to simulate failed compilations.
            try
            {
                Regex invalidRegex = new("[");
            }
            catch (RegexCompilationException)
            {
                // Expected exception for invalid regex pattern.
            }

            // Test with negative cases.
            using Regex negativeRegex = new(@"\d+");
            negativeRegex.IsMatch("no digits here");
        }

        // Ensure any previous allocations are cleaned up.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect(); // Ensure any previous allocations are cleaned up.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect(); // Ensure any previous allocations are cleaned up.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect(); // Ensure any previous allocations are cleaned up.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Capture the difference relative to the initial snapshot.
        dotMemory.Check(memory =>
        {
            var diff = memory.GetDifference(snapshot);

            // Filter objects by type using the documented syntax.
            var regexInstances = diff.GetSurvivedObjects(where => where.Type.Is<Regex>());
            var setInstances = diff.GetSurvivedObjects(where => where.Type.Is<RegexSet>());

            var regexInstancess = memory.GetObjects(where => where.Type.Is<Regex>());
            var setInstancess = memory.GetObjects(where => where.Type.Is<RegexSet>());

            //ObjectSet re2Handle = memory.GetObjects(where => where.Type.Is<Re2Handle>());
            //ObjectSet reHandle = diff.GetSurvivedObjects(where => where.Type.Is<Re2Handle>());

            var regexHandle2 = memory.GetObjects(where => where.Type.Is<RegexHandle>());
            var regexHandle22 = diff.GetSurvivedObjects(where => where.Type.Is<RegexHandle>());

            // Assert that no instances of Regex or RegexSet remain.
            Assert.Equal(0, regexInstancess.ObjectsCount);
            Assert.Equal(0, setInstancess.ObjectsCount);

            // Assert that no new instances of Regex or RegexSet remain.
            Assert.Equal(0, regexInstances.ObjectsCount);
            Assert.Equal(0, setInstances.ObjectsCount);
        });
    }

    [Fact]
    public void MemoryLeakTestWithoutWeakRefs()
    {
        // Ensure any previous allocations are cleaned up.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Record baseline memory usage.
        var memoryBefore = GC.GetTotalMemory(true);

        // Create and use objects in a loop.
        for (var i = 0; i < 10_0000; i++)
        {
            // Create a Regex instance and use it. It will become unreachable after the using block.
            using (Regex regex = new(@"\w+"))
            {
                regex.IsMatch("test string");
            }

            // Create a RegexSet instance and use it.
            string[] patterns = [@"\d+", @"\w+", "[a-z]+", "[A-Z]+", @"\s+", @"\W+", @"\D+"];
            RegexSet set = new(patterns);
            set.Match("123 abc");

            // Test with invalid regex patterns to simulate failed compilations.
            try
            {
                Regex invalidRegex = new("[");
            }
            catch (RegexCompilationException)
            {
                // Expected exception for invalid regex pattern.
            }

            // Test with negative cases.
            using Regex negativeRegex = new(@"\d+");
            negativeRegex.IsMatch("no digits here");
        }

        // Force garbage collection after workload.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Record memory usage after garbage collection.
        var memoryAfter = GC.GetTotalMemory(true);
        var memoryDifference = memoryAfter - memoryBefore;

        // Adjust the threshold as needed; here we use 10 MB as an example.
        Assert.True(memoryDifference < 10 * 1024 * 1024,
            $"Memory increased by {memoryDifference} bytes, which might indicate a leak.");
    }
}
