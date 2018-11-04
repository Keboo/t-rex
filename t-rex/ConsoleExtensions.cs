using System;
using System.CommandLine;
using TRexLib;

namespace TRex.CommandLine
{
    internal static class ConsoleExtensions
    {
        public static ConsoleColor SetColor(this IConsole console, System.ConsoleColor color) => new ConsoleColor(console, color);

        public static ConsoleColor SetColorForOutcome(this IConsole console, TestOutcome outcome)
        {
            switch (outcome)
            {
                case TestOutcome.NotExecuted:
                case TestOutcome.Inconclusive:
                case TestOutcome.Pending:
                    return new ConsoleColor(console, System.ConsoleColor.Yellow);
                case TestOutcome.Failed:
                    return new ConsoleColor(console, System.ConsoleColor.Red);
                case TestOutcome.Passed:
                    return new ConsoleColor(console, System.ConsoleColor.Green);
                case TestOutcome.Timeout:
                    return new ConsoleColor(console, System.ConsoleColor.Magenta);

                default:
                    throw new NotSupportedException();
            }
        }

        public static System.ConsoleColor GetColorForOutcome( this TestOutcome outcome)
        {
            switch (outcome)
            {
                case TestOutcome.NotExecuted:
                case TestOutcome.Inconclusive:
                case TestOutcome.Pending:
                    return System.ConsoleColor.Yellow;
                case TestOutcome.Failed:
                    return System.ConsoleColor.Red;
                case TestOutcome.Passed:
                    return System.ConsoleColor.Green;
                case TestOutcome.Timeout:
                    return System.ConsoleColor.Magenta;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}