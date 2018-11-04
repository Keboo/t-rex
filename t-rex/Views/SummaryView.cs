using System.CommandLine.Rendering.Views;
using TRexLib;

namespace TRex.CommandLine.Views
{
    public sealed class SummaryView : StackLayoutView
    {
        public SummaryView(int passed, int failed, int notRun)
        {
            this.Add("SUMMARY:");
            Add(new StackLayoutView(Orientation.Horizontal)
            {
                new ColoredContentView($"Passed: {passed}, ", TestOutcome.Passed.GetColorForOutcome()),
                new ColoredContentView($"Failed: {failed}, ", TestOutcome.Failed.GetColorForOutcome()),
                new ColoredContentView($"Not Run: {notRun}", TestOutcome.NotExecuted.GetColorForOutcome())
            });
        }
    }
}