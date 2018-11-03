using System.CommandLine.Rendering.Views;
using Microsoft.Recipes;
using TRexLib;

namespace TRex.CommandLine.Views
{
    public sealed class TestResultView : StackLayoutView
    {
        public TestResultView(bool hideTestOutput, TestResult result)
        {
            var durationForTest = result.Duration.IfNotNull().Then(d => d.TotalSeconds).ElseDefault();

            Add(new StackLayoutView(Orientation.Horizontal)
            {
                new ContentView($"{result.TestName}     "),
                new DurationView(durationForTest)
            });

            if (!hideTestOutput &&
                result.Outcome == TestOutcome.Failed)
            {
                if (!string.IsNullOrWhiteSpace(result.Output))
                {
                    Add(new ColoredContentView(result.Output, System.ConsoleColor.Gray));
                }

                if (!string.IsNullOrWhiteSpace(result.Stacktrace))
                {
                    Add(new ColoredContentView("Stack trace:", System.ConsoleColor.DarkGray));

                    Add(new ColoredContentView(result.Stacktrace, System.ConsoleColor.Gray));
                }
            }
        }
    }
}