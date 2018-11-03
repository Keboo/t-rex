using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;

namespace TRex.CommandLine.Views
{
    public sealed class ColoredStackLayoutView : StackLayoutView
    {
        private readonly System.ConsoleColor _color;

        public ColoredStackLayoutView(System.ConsoleColor color)
        {
            _color = color;
        }

        public override void Render(ConsoleRenderer renderer, Region region)
        {
            using (renderer.Console.SetColor(_color))
            {
                base.Render(renderer, region);
            }
        }
    }
}