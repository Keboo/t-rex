using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;

namespace TRex.CommandLine.Views
{
    public class ColoredContentView : ContentView
    {
        private readonly System.ConsoleColor _color;

        public ColoredContentView(string content, System.ConsoleColor color)
            : base(content)
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