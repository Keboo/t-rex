﻿using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;

namespace TRex.CommandLine.Views
{
    public class DurationView : ContentView
    {
        public DurationView(double? value)
            : base($"({value}s)")
        {

        }

        public override void Render(ConsoleRenderer renderer, Region region)
        {
            using (renderer.Console.SetColor(System.ConsoleColor.Gray))
            {
                base.Render(renderer, region);
            }
        }
    }
}